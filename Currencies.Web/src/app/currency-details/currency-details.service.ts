import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


class Data {
  data: any;
}

@Injectable({
  providedIn: 'root',
})
export class CurrencyDetailsService {
  private apiUrl = 'http://localhost:5000/api/';

  constructor(private http: HttpClient) { }
  
  getCurrencyInfo(id){
    return this.http.get<Data>(`${this.apiUrl}currency/${id}`)
  }

  getExchangeInfo(id){
    return this.http.get<Data>(`${this.apiUrl}exchange/from/4/to/${id}`)
  }

  exchangeCurrency(convert){
    return this.http.post<Data>(`${this.apiUrl}user-amount/convert`, convert)
  }

  exchangeHistoryBuy(id){
    return this.http.get<Data>(`${this.apiUrl}exchange/0/${id}`)
  }

  exchangeHistorySell(id){
    return this.http.get<Data>(`${this.apiUrl}exchange/1/${id}`)
  }
}
