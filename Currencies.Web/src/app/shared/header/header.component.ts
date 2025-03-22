import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MenuItem } from 'primeng/api';
import { UserService } from 'src/app/userAuth/user.service';
import { AppService } from '../../app.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  menuItems: MenuItem[];
  loginStr: string;
  passwordStr: string
  
  constructor(
    private appService: AppService,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService
  ) {
     this.menuItems = [
  ];
}

  ngOnInit(): void {

  }

  login(){
    this.router.navigateByUrl('login')
  }

  register(){
    this.router.navigateByUrl('register')
  }

  isLoggedIn(){
    return localStorage.getItem('displayName') != null
  }

  getUserName(){
    return localStorage.getItem('displayName');
  }

  logout(){
    this.userService.logoutUser().subscribe(e => {
      localStorage.clear()
      this.toastr.info(e.message)
    })
  }
}
