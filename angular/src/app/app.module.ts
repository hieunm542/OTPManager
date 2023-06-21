import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CoreModule } from './core/core.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { TokenComponent } from './token/token.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { PhoneComponent } from './phone/phone.component';
import { SmsComponent } from './sms/sms.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatInputModule} from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    TokenComponent,
    PhoneComponent,
    SmsComponent,

  ],
  imports: [
    MatFormFieldModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatDatepickerModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    BrowserModule,
    FormsModule,
    HttpClientModule,
    CoreModule,
    AppRoutingModule,
    CommonModule,
    ReactiveFormsModule

  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule { }
