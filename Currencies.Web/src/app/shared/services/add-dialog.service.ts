import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddDialogService {
  private displayAddDialogSource = new BehaviorSubject<boolean>(false);
  displayDialog$ = this.displayAddDialogSource.asObservable();

  private dataSubject = new BehaviorSubject<any>(null);
  data$ = this.dataSubject.asObservable();

  showDialog(data: any = null) {
    this.dataSubject.next(data);
    this.displayAddDialogSource.next(true);
  }

  hideDialog() {
    this.dataSubject.next(null);
    this.displayAddDialogSource.next(false);
  }
}