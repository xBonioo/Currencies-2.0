import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../shared/services/auth.service';
import { UserRegister } from './register/register.model';

class Data {
  data: any;
  message: any;
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'http://localhost:5000/api/';

  constructor(private http: HttpClient) { }

  loginUser(login, password) {
    return this.http.post<Data>(`${this.apiUrl}user/signin`, {
        username: login,
        password: password,
      })
  }

  logoutUser() {
    return this.http.post<Data>(`${this.apiUrl}user/signout`, null)
  }

  getUserInfo(id) {
    return this.http.post<Data>(`${this.apiUrl}user/get-user`, {
      userId: id
    })
  }

  getUserAmount(id) {
    return this.http.post<Data>(`${this.apiUrl}user/get-user`, {
      userId: id
    })
  }

  registerUser(registerForm: UserRegister) {
    return this.http.post<Data>(`${this.apiUrl}user/register`, registerForm)
  }

  getAccounts(id) {
    return this.http.get<Data>(`${this.apiUrl}user-amount/${id}`)
  }

  addToAccount(obj){
    return this.http.post<Data>(`${this.apiUrl}user-amount/add`, obj)
  }

  refreshToken(token){
    return this.http.post<Data>(`${this.apiUrl}user/refreshtoken`, {refreshToken: token})
  }
}
