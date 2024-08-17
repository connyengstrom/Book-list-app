import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AddEditBookComponent } from './components/add-edit-book/add-edit-book.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './guards/auth.guard';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'add-book', component: AddEditBookComponent, canActivate: [AuthGuard] },
  { path: 'edit-book/:id', component: AddEditBookComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent }
];
