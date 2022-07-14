import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-pending-verification-users',
  templateUrl: './pending-verification-users.component.html',
  styleUrls: ['./pending-verification-users.component.css']
})
export class PendingVerificationUsersComponent implements OnInit {

  deliverers!: User[];

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.userService.getDeliverers().subscribe(
      data => {
        this.deliverers = data;
      }
    )
  }

  onVerify(username: string) {
    this.userService.verifyUser(username).subscribe(
      data => {
        alert('User successfully verified');
        window.location.reload();
      }, error => {
        alert(error);
      }
    )
  }

  declineVerification(username: string) {
    this.userService.declineVerification(username).subscribe(
      data => {
        alert('User verification is declined');
        window.location.reload();
      }, error => {
        alert(error);
      }
    )
  }

}
