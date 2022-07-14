import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FacebookLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';
import { UserForRegister } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registrationForm!: FormGroup;
  user!: UserForRegister;
  userSubmitted: boolean = false;
  googleUser!: SocialUser;

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router, private authService: SocialAuthService) { }

  ngOnInit() {
    localStorage.removeItem('email');
    localStorage.removeItem('token');
    localStorage.removeItem('id');

    this.createRegistrationForm();

    this.authService.authState.subscribe((user) => {
      this.googleUser = user;
    });
  }

  createRegistrationForm() {
    this.registrationForm = this.fb.group({
      userName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(8)]],
      confirmPassword: [null, Validators.required],
      name: [null, Validators.required],
      lastname: [null, Validators.required],
      dateOfBirth: [null, Validators.required],
      address: [null, Validators.required],
      userType: [null, Validators.required],
      userImage: [null, Validators.required]
    }, {validators: this.passwordMatchingValidatior});
  }

  passwordMatchingValidatior(fg: FormGroup): Validators {
    if (fg.get('password')?.value === fg.get('confirmPassword')?.value) {
      return null as any;
    } else {
      return {notmatched: true};
    }
  }

  onSignIn(googleUser: any) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
  }

  onSubmit() {
    this.userSubmitted = true;

    if(this.registrationForm.valid) {
      this.userService.registerUser(this.userData()).subscribe(() => {
        alert("User successfully added")
        this.router.navigate(['']);
      })
    }
  }

  loginWithFb() {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID).then((data) => console.log("Social User From FB ", data))
  }

  userData(): UserForRegister {
    return this.user = {
      username: this.userName.value,
      email: this.email.value,
      password1: this.password.value,
      password2: this.confirmPassword.value,
      name: this.name.value,
      surname: this.lastname.value,
      dateOfBirth: this.dateOfBirth.value,
      address: this.address.value,
      userType: this.userType.value,
      image: this.userImage.value,
    }
  }

  get userName() {
    return this.registrationForm.get('userName') as FormControl;
  }
  get email() {
    return this.registrationForm.get('email') as FormControl;
  }
  get password() {
    return this.registrationForm.get('password') as FormControl;
  }
  get confirmPassword() {
    return this.registrationForm.get('confirmPassword') as FormControl;
  }
  get name() {
    return this.registrationForm.get('name') as FormControl;
  }
  get lastname() {
    return this.registrationForm.get('lastname') as FormControl;
  }
  get dateOfBirth() {
    return this.registrationForm.get('dateOfBirth') as FormControl;
  }
  get address() {
    return this.registrationForm.get('address') as FormControl;
  }
  get userType() {
    return this.registrationForm.get('userType') as FormControl;
  }
  get userImage() {
    return this.registrationForm.get('userImage') as FormControl;
  }

}
