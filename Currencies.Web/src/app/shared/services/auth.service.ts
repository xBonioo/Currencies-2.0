import { Injectable } from '@angular/core';



@Injectable()
export class AuthService {
  setToken(token: string) {  
    localStorage.setItem('token', token);
  }
  
  getToken(): string | null {
    let token = localStorage.getItem('token');
    if (token != null) {
      token = token.toString();
    }
    return token;
  }

  logout(){
    localStorage.clear()
  }
}
