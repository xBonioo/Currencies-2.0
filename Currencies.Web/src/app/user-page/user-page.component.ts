import { Component, OnInit, ViewChild } from '@angular/core';
import { CurrencyModel } from '../currencies/currencies.model';
import { AddDialogService } from '../shared/services/add-dialog.service';
import { UserService } from '../userAuth/user.service';
import { Table } from 'primeng/table';

@Component({
  selector: 'app-user',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.scss']
})
export class UserPageComponent implements OnInit {
  @ViewChild(Table) dt: Table;

  userInfo
  currencies
  userId
  constructor(
    private userSevice: UserService,
    private currencyModel: CurrencyModel,
    private dialogService: AddDialogService
  ) {
    this.userId = localStorage.getItem('id');
    userSevice.getUserInfo(this.userId).subscribe(x=>{
      this.userInfo = x.data;
    })
    userSevice.getAccounts(this.userId).subscribe(x=>{
      this.currencies = x.data
    })
   }

  ngOnInit(): void {
    
  }

  getSymbol(id){
    return this.currencyModel.getCurrencySymbol(id);
  }

  getName(id){
    return this.currencyModel.getCurrencyName(id);
  }

  add(){
    this.dialogService.showDialog()
  }

}
