import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserService } from '../user.service';
import { UserRegister } from './register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  user = new UserRegister()
pesl:string;
  constructor(
    private service: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  register(){
    this.user.identityNumber =  Number.parseInt(this.pesl)
    this.service.registerUser(this.user).subscribe((x) => {
      this.toastr.success("Konto utworzone pomyślnie, możesz się zalogować");
      this.router.navigateByUrl('login');
    },
    error => {
      error.error.BaseResponseError.forEach(element => {
        this.toastr.error(element.Code);
      });
    });
  }
}
