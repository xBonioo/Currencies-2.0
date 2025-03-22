import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrencyExchangePopupComponent } from './currency-exchange-popup.component';

describe('CurrencyExchangePopupComponent', () => {
  let component: CurrencyExchangePopupComponent;
  let fixture: ComponentFixture<CurrencyExchangePopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CurrencyExchangePopupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CurrencyExchangePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
