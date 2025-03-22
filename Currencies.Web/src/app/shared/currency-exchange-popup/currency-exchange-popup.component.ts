import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CurrencyDetailsService } from 'src/app/currency-details/currency-details.service';
import { DialogService } from '../services/dialog.service';

@Component({
  selector: 'app-currency-exchange-popup',
  templateUrl: './currency-exchange-popup.component.html',
  styleUrls: ['./currency-exchange-popup.component.scss']
})
export class CurrencyExchangePopupComponent implements OnInit {
  display: boolean = false;
  currencyFrom: string = '';
  fromAmount: number = 0;
  currencyTo: string = '';
  foreignCurr: string
  data
  buyingCurrency

  constructor(
    private dialogService: DialogService,
    private currencyDetailsService: CurrencyDetailsService,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.dialogService.displayDialog$.subscribe(show => {
      this.display = show;
    });

    this.dialogService.data$.subscribe(data => {
      this.data = data
      this.currencyFrom = this.data?.Data.Item1.FromCurrency.Symbol
      this.currencyTo = this.data?.Data.Item1.ToCurrency.Symbol
      this.foreignCurr = this.currencyTo
      this.buyingCurrency = true
    });
  }

  hideDialog() {
    this.dialogService.hideDialog();
  }

  swap(){
    this.buyingCurrency = !this.buyingCurrency;
    let tmp = this.currencyFrom;
    this.currencyFrom = this.currencyTo;
    this.currencyTo = tmp;
  }

  toAmount(){
    let rate = this.buyingCurrency ? this.data?.Data.Item1.Rate : this.data?.Data.Item2.Rate
    return this.fromAmount * rate
  }

  exchangeCurrency() {
    this.currencyDetailsService.exchangeCurrency({ userId: localStorage.getItem('id'), fromCurrencyId: this.data.Data.Item1.FromCurrency.Id, toCurrencyId: this.data.Data.Item1.ToCurrency.Id, amount: this.fromAmount }).subscribe(
      x =>{
        this.toastr.success("Wymiana zakończona pomyślnie");
        this.hideDialog();
      },
      error =>{
        this.toastr.error(error.error.Message);
        this.hideDialog();
      }
    )
    
  }
}
