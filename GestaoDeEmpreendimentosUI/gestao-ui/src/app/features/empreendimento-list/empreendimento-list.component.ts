import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { EmpreendimentoService, Empreendimento } from '../../services/empreendimento.service';
import { EventService } from '../../services/event.service';

@Component({
  selector: 'empreendimento-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './empreendimento-list.component.html',
  styleUrls: ['../styles.scss']
})
export class EmpreendimentoListComponent {
  loading = signal(false);
  error = signal<string | null>(null);
  data = signal<Empreendimento[]>([]);
  success = signal<string | null>(null);

  constructor(private svc: EmpreendimentoService, private router: Router, private events: EventService) {
    // Carrega a lista ao iniciar o componente.
    this.load();
  }

  // Faz a requisição para o backend e atualiza o estado local.
  load() {
    this.loading.set(true);
    this.error.set(null);
    this.svc.list().subscribe({
      next: (res) => {
        this.data.set(res || []);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erro ao carregar dados.');
        this.loading.set(false);
      }
    });
  }

  // Navega para o formulário de criação.
  novo() {
    this.router.navigate(['/empreendimentos/new']);
  }

  // Navega para o formulário de edição do registro selecionado.
  editar(id?: number) {
    if (!id) return;
    this.router.navigate([`/empreendimentos/${id}/edit`]);
  }

  // Inativa um empreendimento e atualiza a lista.
  deletar(id?: number) {
    if (!id) return;
    if (!confirm('Confirma a inativação?')) return;
    this.loading.set(true);
    this.svc.delete(id).subscribe({
      next: () => {
        this.success.set('Inativado com sucesso.');
        this.events.notifyChange();
        this.load();
      },
      error: () => {
        this.error.set('Erro ao inativar.');
        this.loading.set(false);
      }
    });
  }
}

