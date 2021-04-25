import { Component, OnInit } from '@angular/core';
import * as oidc from 'oidc-client';

@Component({
  selector: 'app-silent-callback',
  templateUrl: './silent-callback.component.html',
  styleUrls: ['./silent-callback.component.css']
})
export class SilentCallbackComponent implements OnInit {

  constructor() { }

  config: oidc.UserManagerSettings = {
    authority: "https://localhost:1000",
    client_id: "AngularClient",
    response_type: "code",
    scope: "Garanti.Write Garanti.Read profile openid email Roles",
  }

  ngOnInit(): void {
    new oidc.UserManager(this.config).signinSilentCallback().then(() => console.log("yeni access token"))
      .catch(error => console.log(error));
  }

}
