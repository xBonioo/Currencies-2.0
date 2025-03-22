import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './shared/header/header.component';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { CurrenciesComponent } from './currencies/currencies.component';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms'; 
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './userAuth/login/login.component';
import { RegisterComponent } from './userAuth/register/register.component';
import { InputTextModule } from 'primeng/inputtext';
import { CurrencyDetailsComponent } from './currency-details/currency-details.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './shared/interceptors/auth.interceptor';
import { AuthService } from './shared/services/auth.service';
import { CalendarModule } from 'primeng/calendar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { CommonModule, DatePipe } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CurrencyExchangePopupComponent } from './shared/currency-exchange-popup/currency-exchange-popup.component';
import { DialogModule } from 'primeng/dialog';
import { UserPageComponent } from './user-page/user-page.component';
import { AddToAccountPopupComponent } from './shared/add-to-account-popup/add-to-account-popup.component';
import { DropdownModule } from 'primeng/dropdown';
import { InputSwitchModule } from 'primeng/inputswitch';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    CurrenciesComponent,
    LoginComponent,
    RegisterComponent,
    CurrencyDetailsComponent,
    CurrencyExchangePopupComponent,
    UserPageComponent,
    AddToAccountPopupComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MenubarModule,
    ButtonModule,
    TableModule,
    FormsModule,
    HttpClientModule,
    InputTextModule,
    CalendarModule,
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    ChartModule,
    DialogModule,
    DropdownModule,
    InputSwitchModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    AuthService,
    DatePipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
