import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { filter, switchMap, take, catchError } from 'rxjs/operators';
import { UserService } from 'src/app/userAuth/user.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private tryingRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private authService: AuthService,
    private userService: UserService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const myToken = this.authService.getToken();
    if (myToken) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${myToken}`,
          'Content-Type': 'application/json',
        }
      });
    }
    return next.handle(request).pipe(catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        const tokenExpired = error.headers.get('token-expired');
        if (tokenExpired) {
          return this.handle401Error(request, next);
        }
        this.authService.logout();
        return throwError(error);
      } else {
        return throwError(error);
      }
    }));
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.tryingRefreshing) {
      this.tryingRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.userService.refreshToken(localStorage.getItem('token')).pipe(
        switchMap((token: any) => {
          this.tryingRefreshing = false;
          this.refreshTokenSubject.next(token);
          localStorage.setItem('token', token);
          return next.handle(this.addAuthorization(request, token));
        }));

    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(jwt => {
          return next.handle(this.addAuthorization(request, jwt));
        }));
    }
  }

  addAuthorization(httpRequest: HttpRequest<any>, token: string) {
    return httpRequest = httpRequest.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

}
