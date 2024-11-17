import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastrService);
  let router = inject(Router);
  return next(req).pipe(catchError(
    error => {

      if (error) {
        console.log("interceptor log");
        console.log(error);
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const modelErr = [];
              for (const key in error.error.errors) {
                if (error.error.errors[key]) modelErr.push(error.error.errors[key]);
               
              }
              for(let err of modelErr.flat() )toast.error(err); ;
            }
            else {
              toast.error(error.error);
            }
            break;
          case 401:
            if(error.error)toast.error(error.error, error.status);
            else toast.error("unauthorized", error.status);
            break;
          case 404:
            toast.error("Not found");
            break;
          case 500:
            const navExtras : NavigationExtras ={state :{error:error.error}};
            router.navigateByUrl("server-error",navExtras);
            break;
          default:
            toast.error("Something unexpected went wrong");
            break;
        }
      }
      throw error;
    }
  ));
};
