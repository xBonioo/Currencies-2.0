import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CurrencyExchangePopupComponent } from '../shared/currency-exchange-popup/currency-exchange-popup.component';
import { DialogService } from '../shared/services/dialog.service';
import { CurrencyDetailsService } from './currency-details.service';

@Component({
  selector: 'app-currency-details',
  templateUrl: './currency-details.component.html',
  styleUrls: ['./currency-details.component.scss']
})
export class CurrencyDetailsComponent implements OnInit {

  data: any;
  options: any;
  currencyInfo: any;
  exchangeInfo: any;
  constructor(
    private service: CurrencyDetailsService,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private dialogService: DialogService,
    private datepipe: DatePipe
  ) { }

  ngOnInit(): void {
    var currencyId = this.route.snapshot.paramMap.get('id');
    this.service.getCurrencyInfo(this.route.snapshot.paramMap.get('id')).subscribe(x => {
      this.currencyInfo = x;
      this.service.exchangeHistoryBuy(currencyId).subscribe(x => {
        this.service.exchangeHistorySell(currencyId).subscribe(y => {
          this.data = {
            labels: x.data.map(a => this.datepipe.transform(a.createdOn, 'yyyy-MM-dd HH:mm')),
            datasets: [
              {
                label: `${this.currencyInfo.data.symbol} to PLN`,
                data: x.data.map(a => a.rate),
                fill: false,
                borderColor: '#4bc0c0'
              },
              {
                label: `PLN to ${this.currencyInfo.data.symbol}`,
                data: y.data.map(a => a.rate),
                fill: false,
                borderColor: '#565656'
              }
            ]
          }
        })
      })
        this.options = {
          title: {
            display: true,
            text: 'Currency Rate History (Base: PLN)',
            fontSize: 16
          },
          legend: {
            position: 'bottom'
          }
        }
      })
      this.service.getExchangeInfo(this.route.snapshot.paramMap.get('id')).subscribe(x => {
        this.exchangeInfo = x
      })
    }

  exchange() {
      if(localStorage.getItem('token') == null) {
      this.toastr.error("Żeby skorzystać z tej funkcji musisz być zalogowany");
      return;
    }
    this.dialogService.showDialog(this.exchangeInfo)
  }

}
