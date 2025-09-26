import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth-service';

export const AuthInterceptor: HttpInterceptorFn = (req, next) => {

    const route = inject(Router);
    const authService = inject(AuthService);
    const authToken = authService.getToken();
    const skipUrls = [
        '/auth/login',
        '/auth/register'
    ];

    const shouldSkip = skipUrls.some(url => req.url.includes(url));

    if (shouldSkip) {
        // Don't add token or redirect — just proceed
        return next(req);
    }

    if(!authToken) {
        //If token not found rediret to login page.
        route.navigate(['/']);

        //Optionally cancel the request.
        //return next(req.clone({url:'about:blank'}));
    } else {
        //Add token to header
        const clonedRequest = req.clone({
            setHeaders: {
                Authorization: `Bearer ${authToken}`
            }
        });
        return next(clonedRequest);
    }
    return next(req);
};
