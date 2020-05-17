import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../../models/RegisterModel';
import { LoginModel } from 'src/app/models/LoginModel';
import { AuthenticationService } from 'src/app/services/auth/authentication.service';
import { take } from 'rxjs/operators';
import { BaseService } from 'src/app/services/base.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  registerModel: RegisterModel = new RegisterModel();
  loginModel: LoginModel = new LoginModel();

  constructor(private authenticationService: AuthenticationService, private router: Router) { }

  ngOnInit() {
  }


  login() {
    console.log(this.loginModel);
    this.authenticationService.Login(this.loginModel).pipe(take(1)).subscribe(r => {
      if (r.success) {
        this.authenticationService.setUser(r.response);
        this.router.navigateByUrl('ChatRoom');
      } else {
        alert(r.message);
      }
    });
  }

  register() {
    this.authenticationService.Register(this.registerModel).pipe(take(1)).subscribe(r => {
      if (r.success) {
        alert('user registered with success.');
      } else {
        alert(r.message);
      }
    });
  }

}
