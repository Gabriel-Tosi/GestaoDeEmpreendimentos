import { Routes } from '@angular/router';

import { EmpreendimentoListComponent } from './features/empreendimento-list/empreendimento-list.component';
import { EmpreendimentoFormComponent } from './features/empreendimento-form/empreendimento-form.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';

// Definição das rotas da aplicação.
// Redireciona a rota raiz para a lista de empreendimentos e trata rotas inválidas.
export const routes: Routes = [
	{ path: '', redirectTo: 'empreendimentos', pathMatch: 'full' },
	{ path: 'empreendimentos', component: EmpreendimentoListComponent },
	{ path: 'dashboard', component: DashboardComponent },
	{ path: 'empreendimentos/new', component: EmpreendimentoFormComponent },
	{ path: 'empreendimentos/:id/edit', component: EmpreendimentoFormComponent },
	{ path: '**', redirectTo: 'empreendimentos' }
];
