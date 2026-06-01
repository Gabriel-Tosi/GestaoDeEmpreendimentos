import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class EventService {
  // Stream usada para notificar outros componentes quando o estado do backend muda.
  private _changes = new Subject<void>();
  public changes$ = this._changes.asObservable();

  // Chame este método sempre que um CRUD alterar dados para forçar atualizações.
  notifyChange() {
    this._changes.next();
  }
}
