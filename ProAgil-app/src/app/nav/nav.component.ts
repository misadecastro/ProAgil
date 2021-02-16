import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public router: Router,
    public toastr: ToastrService,
    public auth: AuthService) { }

  ngOnInit() {
  }

  loggedIn(){
    return this.auth.loggedIn();
  }

  logout(){
    localStorage.removeItem('token');
    this.router.navigate(['/user/login'])
  }

  entrar(){
    this.router.navigate(['/user/login'])
  }

  userName(){
    return sessionStorage.getItem('userName')
  }

}
