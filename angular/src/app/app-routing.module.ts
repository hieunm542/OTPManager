import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './core';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { DemoApisComponent } from './demo-apis/demo-apis.component';
import { TokenComponent } from './token/token.component';
import { PhoneComponent } from './phone/phone.component';
import { SmsComponent } from './sms/sms.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: HomeComponent,
    canActivate: [AuthGuard],
  },
  { path: 'login', component: LoginComponent },
  { path: 'demo-apis', component: DemoApisComponent, canActivate: [AuthGuard] },
  { path: 'tokens', component: TokenComponent, canActivate: [AuthGuard] },
  { path: 'phones', component: PhoneComponent, canActivate: [AuthGuard] },
  { path: 'sms', component: SmsComponent, canActivate: [AuthGuard] },
  {
    path: 'management',
    loadChildren: () =>
      import('./management/management.module').then((m) => m.ManagementModule),
    canActivate: [AuthGuard],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
