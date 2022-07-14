import { Component, Input, OnInit } from '@angular/core';
import { PasswordChange, User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  @Input() userId!: number;
  @Input() userData!: User;
  image!: string;
  changePassword: PasswordChange = {currentPassword: '', newPassword: ''};

  constructor(private userService: UserService) { }

  ngOnInit() {
  }

  editProfile() {
    this.userData.image = this.getImageName(this.userData.image);

    this.userService.editUser(this.userData, this.userId).subscribe(
      data => {
        alert("User successfully updated");
      }, error => {
        alert(error);
      }
    )
   }

  getImageName(image: string) {
    var temp = image.split('\\');
    return temp[temp.length - 1];
  }

  onPasswordSubmit() {
    if(this.changePassword.newPassword.length >= 8) {
      this.userService.changePassword(this.changePassword, this.userId).subscribe(
        data => {
          alert("Password successfully changed");
          window.location.reload();
        }, error => {
          alert(error);
        }
      )
    } else {
      alert("New password must have minimum 8 character");
    }
  }
}
