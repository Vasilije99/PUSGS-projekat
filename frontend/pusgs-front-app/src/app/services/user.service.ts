import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserForRegister, UserForLogin, User, PasswordChange } from '../model/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  registerUser(user: UserForRegister) {
    return this.http.post(this.baseUrl + '/User/register', user);
  }

  loginUser(user: UserForLogin) {
    return this.http.post(this.baseUrl + '/User/login', user);
  }

  getUserData(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + '/User/getUser/' + id);
  }

  editUser(newUser: User, id: number) {
    return this.http.put(this.baseUrl + '/User/updateUser/' + id, newUser);
  }

  changePassword(pc: PasswordChange, id: number) {
    return this.http.put(this.baseUrl + '/User/passwordChange/' + id, pc);
  }

  verificationRequest(id: number): Observable<User> {
    return this.http.put<User>(this.baseUrl + '/User/verificationRequest/' + id, null);
  }

  getDeliverers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + '/User/deliverers');
  }

  verifyUser(username: string) {
    return this.http.put(this.baseUrl + '/User/verifyUser/' + username, null);
  }

  declineVerification(username: string) {
    return this.http.put(this.baseUrl + '/User/declineVerification/' + username, null);
  }
}
