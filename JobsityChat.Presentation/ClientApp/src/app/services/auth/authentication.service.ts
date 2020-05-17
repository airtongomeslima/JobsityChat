import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { BaseResponseModel } from 'src/app/models/baseResponseModel';
import { AuthenticationModel } from 'src/app/models/AuthenticationModel';
import { LoginModel } from 'src/app/models/LoginModel';
import { RegisterModel } from 'src/app/models/RegisterModel';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private baseService: BaseService) { }

  public Login(login: LoginModel): Observable<BaseResponseModel<AuthenticationModel>> {
    this.cleanUser();
    return this.baseService.post<AuthenticationModel, LoginModel>(login, 'Authentication/Login');
  }

  public Register(register: RegisterModel): Observable<BaseResponseModel<boolean>> {
    this.cleanUser();
    return this.baseService.post<boolean, RegisterModel>(register, 'Authentication/Register');
  }

  public setUser(authModel: AuthenticationModel) {
    this.baseService.setLocalStorage('currentUser', authModel.access_token);
  }

  public cleanUser() {
    this.baseService.removeLocalStorage('currentUser');
  }

}
