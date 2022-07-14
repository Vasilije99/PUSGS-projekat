import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators,FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { UserForLogin } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  user!: UserForLogin;
  loginForm!: FormGroup;

  constructor(private userService: UserService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    this.createLoginForm();

    localStorage.removeItem('email');
    localStorage.removeItem('token');
    localStorage.removeItem('id');
  }

  createLoginForm() {
    this.loginForm = this.fb.group({
      email: [null, Validators.required],
      password: [null, Validators.required]
    })
  }

  onLogin() {
    if(this.loginForm.valid) {
      this.userService.loginUser(this.userData()).subscribe(
        data => {
          let newUser: any;
          newUser = data;
          localStorage.setItem('email', newUser.email);
          localStorage.setItem('token', newUser.token);
          localStorage.setItem('id', newUser.id);
          this.router.navigate(['/dashboard/' + newUser.id]);
        }, error => {
          alert("Some error has occurred")
        }
      )
    } else {
      alert("Form is not valid")
    }
  }

  userData(): UserForLogin {
    return this.user = {
      email: this.email.value,
      password: this.password.value
    }
  }

  get email() {
    return this.loginForm.get('email') as FormControl;
  }
  get password() {
    return this.loginForm.get('password') as FormControl;
  }
}
