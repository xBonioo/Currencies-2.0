import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AppService {
  private apiUrl = 'http://localhost:5000/api/';

  constructor(private http: HttpClient) {}
  login(login: string, password: string) {
    
    let obj = {
      Username: login,
      Password: password
    }
    return this.http.post(`${this.apiUrl}user/signin`, obj);
  }
}
