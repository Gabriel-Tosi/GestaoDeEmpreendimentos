import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Empreendimento {
  id?: number;
  nome: string;
  cnpj?: string;
  endereco?: string;
  ativo?: boolean;
}

// Payload usado para criar um novo empreendimento.
export type EmpreendimentoCreatePayload = Omit<Empreendimento, 'id' | 'ativo'>;
// Payload parcial usado para atualizar um empreendimento existente.
export type EmpreendimentoUpdatePayload = Partial<Omit<Empreendimento, 'id' | 'ativo'>>;

@Injectable({ providedIn: 'root' })
export class EmpreendimentoService {
  private baseUrl = '/api/empreendimentos';

  constructor(private http: HttpClient) {}

  // Retorna todos os empreendimentos.
  list(): Observable<Empreendimento[]> {
    return this.http.get<Empreendimento[]>(this.baseUrl);
  }

  // Retorna apenas os empreendimentos ativos.
  listAtivos(): Observable<Empreendimento[]> {
    return this.http.get<Empreendimento[]>(`${this.baseUrl}/ativos`);
  }

  // Retorna apenas os empreendimentos inativos.
  listInativos(): Observable<Empreendimento[]> {
    return this.http.get<Empreendimento[]>(`${this.baseUrl}/inativos`);
  }

  // Busca um empreendimento pelo ID.
  get(id: number): Observable<Empreendimento> {
    return this.http.get<Empreendimento>(`${this.baseUrl}/${id}`);
  }

  // Cria um novo empreendimento.
  create(payload: EmpreendimentoCreatePayload): Observable<Empreendimento> {
    return this.http.post<Empreendimento>(this.baseUrl, payload);
  }

  // Atualiza um empreendimento existente.
  update(id: number, payload: EmpreendimentoUpdatePayload): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  // Inativa (soft delete) um empreendimento.
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
