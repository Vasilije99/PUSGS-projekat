import { Component, OnInit, Input} from '@angular/core';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-verification',
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.css']
})
export class VerificationComponent implements OnInit {

  @Input() userId!: number;
  @Input() verificationUser!: User;

  constructor(private userService: UserService) { }

  ngOnInit() {
  }

  sendVerificationRequest() {
    this.userService.verificationRequest(this.userId).subscribe(
      data => {
        this.verificationUser = data;
      }, error => {
        alert(error);
      }
    )
  }

}
