import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { SocialLoginModule, SocialAuthServiceConfig  } from 'angularx-social-login';
import { FacebookLoginProvider } from 'angularx-social-login';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { RegisterComponent } from './components/register/register.component';
import { UserService } from './services/user.service';
import { HttpClientModule } from '@angular/common/http';
import { VerificationComponent } from './components/user-profile/verification/verification.component';
import { PendingVerificationUsersComponent } from './components/pending-verification-users/pending-verification-users.component';
import { AllOrdersComponent } from './components/all-orders/all-orders.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { CreateOrderComponent } from './components/create-order/create-order.component';
import { UserFinishedOrdersComponent } from './components/user-finished-orders/user-finished-orders.component';
import { PendingOrdersComponent } from './components/pending-orders/pending-orders.component';
import { DelivererOrdersComponent } from './components/deliverer-orders/deliverer-orders.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { UserProfileComponent } from './components/user-profile/user-profile.component';
import { CountdownModule } from 'ngx-countdown';
import { MapComponent } from './components/map/map.component';

const appRoutes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'dashboard/:id', component: DashboardComponent},

  {path: '**', component: PageNotFoundComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    NavBarComponent,
    DashboardComponent,
    UserProfileComponent,
    VerificationComponent,
    PendingVerificationUsersComponent,
    AllOrdersComponent,
    AddProductComponent,
    CreateOrderComponent,
    UserFinishedOrdersComponent,
    PendingOrdersComponent,
    DelivererOrdersComponent,
    MapComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
    SocialLoginModule,
    CountdownModule
  ],
  providers: [
    UserService,
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider('749091429673983')
          }
        ],
        onError: (err) => {
          console.error(err);
        }
      } as SocialAuthServiceConfig,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
