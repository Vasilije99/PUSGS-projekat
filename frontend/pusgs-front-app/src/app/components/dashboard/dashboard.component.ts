import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  userId!: number;
  userData!: User;
  tab: string = 'myProfile';

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.userId = +this.route.snapshot.params['id'];
    if(localStorage.getItem('id') === this.userId.toString()) {
      this.userService.getUserData(this.userId).subscribe(
        data => {
          this.userData = data;
        }, error => {
            alert(error);
        }
      )
    }
  }

  onNavChange(activeTab: string) {
    this.tab = activeTab;
  }
}
