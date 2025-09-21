import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Checkout } from './components/checkout/checkout';
import { Login } from './components/login/login';
import { Signup } from './components/signup/signup';
import { Dashboard } from './components/dashboard/dashboard';
import { Myorders } from './components/myorders/myorders';
import { AddProduct } from './components/add-product/add-product';
import { HttpClientModule, provideHttpClient, withInterceptors } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor';
import { CartItems } from './components/checkout/cart-items/cart-items';
import { Confirmation } from './components/confirmation/confirmation';
import { Allorders } from './components/allorders/allorders';

@NgModule({
  declarations: [
    App,
    Checkout,
    Login,
    Signup,
    Dashboard,
    Myorders,
    AddProduct,
    CartItems,
    Confirmation,
    Allorders
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors([AuthInterceptor]))
  ],
  bootstrap: [App]
})
export class AppModule { }
