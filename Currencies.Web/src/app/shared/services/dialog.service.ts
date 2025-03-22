import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  private displayDialogSource = new BehaviorSubject<boolean>(false);
  displayDialog$ = this.displayDialogSource.asObservable();

  private dataSubject = new BehaviorSubject<any>(null);
  data$ = this.dataSubject.asObservable();

  showDialog(data: any = null) {
    this.dataSubject.next(data);
    this.displayDialogSource.next(true);
  }

  hideDialog() {
    this.dataSubject.next(null);
    this.displayDialogSource.next(false);
  }
}