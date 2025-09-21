import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Dashboard } from './components/dashboard/dashboard';
import { Checkout } from './components/checkout/checkout';
import { Login } from './components/login/login';
import { Signup } from './components/signup/signup';
import { Myorders } from './components/myorders/myorders';
import { AddProduct } from './components/add-product/add-product';
import { AuthGuard } from './services/auth.guard';
import { Confirmation } from './components/confirmation/confirmation';
import { Allorders } from './components/allorders/allorders';

const routes: Routes = [
  { path: '', component: Login },
  { path: 'signup', component: Signup },

  { path: 'home', component: Dashboard , canActivate: [AuthGuard] },
  { path: 'checkout', component: Checkout , canActivate: [AuthGuard] },
  { path: 'myorders', component: Myorders , canActivate: [AuthGuard] },
  { path: 'addproduct', component: AddProduct, canActivate: [AuthGuard] },
  { path: 'pendingorders', component: Allorders, canActivate: [AuthGuard] },
  { path: 'confirm/:id', component: Confirmation }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
