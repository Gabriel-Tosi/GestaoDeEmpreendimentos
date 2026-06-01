import { Component, signal, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmpreendimentoService } from '../../services/empreendimento.service';
import { EventService } from '../../services/event.service';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['../styles.scss']
})
export class DashboardComponent implements OnDestroy {
  ativos = signal(0);
  inativos = signal(0);
  private subs = new Subscription();

  constructor(private svc: EmpreendimentoService, private events: EventService) {
    this.load();
    // Atualiza imediatamente quando há mudanças em outro componente.
    this.subs.add(this.events.changes$.subscribe(() => this.load()));
    // Atualiza periodicamente para manter os dados sincronizados.
    this.subs.add(timer(20000, 20000).subscribe(() => this.load()));
  }

  // Carrega os totais de ativos e inativos do backend.
  load() {
    this.svc.listAtivos().subscribe({ next: (res) => this.ativos.set(res.length), error: () => this.ativos.set(0) });
    this.svc.listInativos().subscribe({ next: (res) => this.inativos.set(res.length), error: () => this.inativos.set(0) });
  }

  ngOnDestroy(): void {
    // Cancela assinaturas ao destruir o componente.
    this.subs.unsubscribe();
  }
}
