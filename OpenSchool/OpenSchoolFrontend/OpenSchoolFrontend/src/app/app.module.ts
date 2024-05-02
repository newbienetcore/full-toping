import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// auth
import { AngularFireModule } from '@angular/fire/compat';
import { AngularFireAuthModule } from '@angular/fire/compat/auth';
// Page Route
import { AppRoutingModule } from './app-routing.module';
import { LayoutsModule } from './admin/components/layouts/layouts.module';
import { ToastrModule } from 'ngx-toastr';

// Laguage
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

// Store
// Store
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
// component
import { AppComponent } from './app.component';
import { AuthlayoutComponent } from './admin/components/authlayout/authlayout.component';
import { environment } from 'src/environments/environment';
import { AnalyticsEffects } from './admin/store/analytics/analytics.effects';
import { rootReducer } from './admin/store';
import { fakebackendInterceptor } from './shared/helpers/fake-backend';
import { ErrorInterceptor } from './shared/helpers/error.interceptor';
import { JwtInterceptor } from './shared/helpers/jwt.interceptor';
import { CRMEffects } from './admin/store/crm/crm.effects';
import { ECoReducer } from './admin/store/ecommerce/ecommerce.reducer';
import { ECoEffects } from './admin/store/ecommerce/ecommerce.effects';
import { LearningEffects } from './admin/store/learning/learning.effects';
import { RealEffects } from './admin/store/real-estate/real-estate.effects';
import { AppRealestateEffects } from './admin/store/app-realestate/apprealestate.effects';
import { AgentEffects } from './admin/store/agent/agent.effects';
import { AgenciesEffects } from './admin/store/agency/agency.effects';
import { TicketEffects } from './admin/store/tickets/ticket.effects';
import { ChatEffects } from './admin/store/chat/chat.effects';
import { ProductEffects } from './admin/store/product/product.effect';
import { InvoiceEffects } from './admin/store/invoices/invoices.effects';
import { AuthenticationEffects } from './admin/store/authentication/authentication.effects';
import { initFirebaseBackend } from './shared/utility/auth-utility';
import { SellerEffects } from './admin/store/seller/seller.effects';
import { OrdersEffects } from './admin/store/orders/order.effects';
import { InstructorEffects } from './admin/store/learning-instructor/instructor.effects';
import { CustomerEffects } from './admin/store/customer/customer.effects';
import { studentsEffects } from './admin/store/students/student.effcts';
import { CourcesEffects } from './admin/store/learning-cources/cources.effect';

export function createTranslateLoader(http: HttpClient): any {
  return new TranslateHttpLoader(http, 'assets/i18n/', '.json');
}
if (environment.defaultauth === 'firebase') {
  initFirebaseBackend(environment.firebaseConfig);
} else {
  fakebackendInterceptor;
}

@NgModule({
  declarations: [
    AppComponent,
    AuthlayoutComponent
  ],
  imports: [
    TranslateModule.forRoot({
      defaultLanguage: 'en',
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    StoreModule.forRoot(rootReducer),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
      logOnly: environment.production, // Restrict extension to log-only mode
    }),

    EffectsModule.forRoot([
      AnalyticsEffects,
      CRMEffects,
      ECoEffects,
      LearningEffects,
      RealEffects,
      AppRealestateEffects,
      AgentEffects,
      AgenciesEffects,
      TicketEffects,
      ChatEffects,
      ProductEffects,
      InvoiceEffects,
      AuthenticationEffects,
      SellerEffects,
      OrdersEffects,
      InstructorEffects,
      CustomerEffects,
      studentsEffects,
      CourcesEffects,
      InstructorEffects
    ]),
    AngularFireModule.initializeApp(environment.firebaseConfig),
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    LayoutsModule,
    ToastrModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    AngularFireAuthModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: fakebackendInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
