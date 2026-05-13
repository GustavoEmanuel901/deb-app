import { Routes } from '@angular/router';
import { DebtFormComponent } from './components/debt-form/debt-form.component';
import { DebtListComponent } from './components/debt-list/debt-list.component';

export const routes: Routes = [
  { path: '', redirectTo: '/debts', pathMatch: 'full' },
  { path: 'debts', component: DebtListComponent },
  { path: 'new-debt', component: DebtFormComponent },
];
