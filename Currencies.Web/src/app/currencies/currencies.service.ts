import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

class Currency{
  data
}

@Injectable({
  providedIn: 'root'
})
export class CurrenciesService {
  private apiUrl = 'http://localhost:5000/api/';

  constructor(private http: HttpClient) { }

  getCurrencies() {
    return this.http.get<Currency>(`${this.apiUrl}currency?PageNumber=1&PageSize=50`);
  }
}
