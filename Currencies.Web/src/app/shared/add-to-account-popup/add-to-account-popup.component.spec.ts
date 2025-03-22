import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddToAccountPopupComponent } from './add-to-account-popup.component';

describe('AddToAccountPopupComponent', () => {
  let component: AddToAccountPopupComponent;
  let fixture: ComponentFixture<AddToAccountPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddToAccountPopupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddToAccountPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
