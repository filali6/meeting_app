import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import {NgxSpinnerModule} from 'ngx-spinner';
import { errorInterceptor } from './_interceptors/error.interceptor';
import { addTokenInterceptor } from './_interceptors/add-token.interceptor';
import { spinnerInterceptor } from './_interceptors/spinner.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
     provideRouter(routes),
     provideHttpClient(withInterceptors([addTokenInterceptor,errorInterceptor,spinnerInterceptor])),
     provideAnimations(),
     provideToastr({positionClass:"toast-bottom-right"}),
     importProvidersFrom(NgxSpinnerModule)
    ]
};
