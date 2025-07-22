import { Routes } from '@angular/router';
import { ListComponent } from './pages/sales/list/list.component';
import { ViewComponent } from './pages/sales/view/view.component';
import { EditComponent } from './pages/sales/edit/edit.component';
import { NewComponent } from './pages/sales/new/new.component';

export const routes: Routes = [
  { path: 'sales/list', component: ListComponent },
  { path: 'sales/new', component: NewComponent },
  { path: 'sales/view/:id', component: ViewComponent },
  { path: 'sales/edit/:id', component: EditComponent },
  { path: '', redirectTo: '/sales/list', pathMatch: 'full' },
  { path: '**', redirectTo: '/sales/list' }
];
