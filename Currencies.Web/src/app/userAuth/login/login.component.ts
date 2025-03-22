import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UserService } from '../user.service';



@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  login: string;
  password: string;
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) { }
  
  ngOnInit(): void {
  }

  loginUser(){
    this.userService.loginUser(this.login, this.password).subscribe((x) => {
        this.authService.setToken(x.data.accessToken);
        localStorage.setItem('id', x.data.userId);
        this.userService.getUserInfo(x.data.userId).subscribe((y)=>{
          localStorage.setItem('displayName', y.data.userName);
        })
        this.toastr.success(x.message)
        this.router.navigateByUrl('#')
      },
    error => {
        this.toastr.error('Błędny login lub hasło');
    });
  }
}
