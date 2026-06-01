import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { EmpreendimentoService, EmpreendimentoCreatePayload, EmpreendimentoUpdatePayload } from '../../services/empreendimento.service';
import { EventService } from '../../services/event.service';

@Component({
  selector: 'empreendimento-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './empreendimento-form.component.html',
  styleUrls: ['../styles.scss']
})
export class EmpreendimentoFormComponent {
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal<string | null>(null);

  form: any;

  id: number | null = null;

  constructor(
    private fb: FormBuilder,
    private svc: EmpreendimentoService,
    private router: Router,
    private route: ActivatedRoute,
    private events: EventService
  ) {
    // Define o formulário reativo com validações básicas.
    this.form = this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3)]],
      cnpj: ['', [Validators.required, Validators.minLength(1)]],
      endereco: [''],
      ativo: [{ value: false, disabled: true }]
    });

    // Se existir um ID na rota, carrega os dados para edição.
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = Number(idParam);
      this.load(this.id);
    }
  }

  // Busca dados do empreendimento para preencher o formulário no modo edição.
  load(id: number) {
    this.loading.set(true);
    this.svc.get(id).subscribe({
      next: (res) => {
        this.form.patchValue({
          nome: (res as any).nome,
          cnpj: (res as any).cnpj,
          endereco: (res as any).endereco,
          ativo: (res as any).ativo
        });
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erro ao carregar registro.');
        this.loading.set(false);
      }
    });
  }

  // Identifica se o registro existente está inativo, para bloquear edições.
  get isInactive() {
    return this.id !== null && this.form.controls['ativo'].value === false;
  }

  // Envia o formulário para criar ou atualizar, conforme o contexto.
  submit() {
    this.error.set(null);
    if (this.form.invalid) return this.error.set('Corrija os erros do formulário.');
    if (this.isInactive) return this.error.set('Empreendimento inativo não pode ser editado.');
    this.loading.set(true);

    const raw = this.form.value;
    const payload: any = {
      nome: raw.nome?.trim() ?? undefined,
      cnpj: raw.cnpj?.trim() ?? undefined,
      endereco: raw.endereco?.trim() ?? undefined
    };

    if (this.id) {
      const updatePayload: EmpreendimentoUpdatePayload = {
        nome: payload.nome,
        cnpj: payload.cnpj,
        endereco: payload.endereco
      };
      this.svc.update(this.id, updatePayload).subscribe({
        next: () => {
          this.success.set('Atualizado com sucesso.');
          this.loading.set(false);
          this.events.notifyChange();
          setTimeout(() => this.router.navigate(['/empreendimentos']), 800);
        },
        error: (err) => {
          this.error.set(err?.error?.message ?? 'Erro ao atualizar.');
          this.loading.set(false);
        }
      });
    } else {
      const createPayload: EmpreendimentoCreatePayload = {
        nome: payload.nome,
        cnpj: payload.cnpj,
        endereco: payload.endereco
      };
      this.svc.create(createPayload).subscribe({
        next: () => {
          this.success.set('Criado com sucesso.');
          this.loading.set(false);
          this.events.notifyChange();
          setTimeout(() => this.router.navigate(['/empreendimentos']), 800);
        },
        error: (err) => {
          this.error.set(err?.error?.message ?? 'Erro ao criar.');
          this.loading.set(false);
        }
      });
    }
  }

  // Volta para a lista de empreendimentos.
  voltar() {
    this.router.navigate(['/empreendimentos']);
  }
}
