import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CurrenciesService } from 'src/app/currencies/currencies.service';
import { UserService } from 'src/app/userAuth/user.service';
import { AddDialogService } from '../services/add-dialog.service';


@Component({
  selector: 'app-add-to-account-popup',
  templateUrl: './add-to-account-popup.component.html',
  styleUrls: ['./add-to-account-popup.component.scss']
})
export class AddToAccountPopupComponent implements OnInit {
  currencyOptions: any[]
  selectedCurrency
  amount: number;
  display: boolean = false;

  constructor(
    private dialogService: AddDialogService,
    private currenciesService: CurrenciesService,
    private userService: UserService,
    private toastr: ToastrService
  ) {
    this.dialogService.displayDialog$.subscribe(show => {
      this.display = show;
    });
  }

  ngOnInit(): void {
    this.currenciesService.getCurrencies().subscribe(x => {
      let arr = x.data.items
      this.currencyOptions = arr.map((currency) => ({
        label: currency.symbol,
        value: currency.id
      }))
    })
  }

  hideDialog() {
    this.dialogService.hideDialog();
  }

  addMoney() {
    this.userService.addToAccount({userId: localStorage.getItem('id'), currencyId: this.selectedCurrency, amount: this.amount, isActive: true}).subscribe(x => {
      this.toastr.success(x.message);
      this.dialogService.hideDialog();
      window.location.reload();
    },
  error => {
    error.error.BaseResponseError.forEach(element => {
      this.toastr.error(element.Code);
    });
  });
  }
}
