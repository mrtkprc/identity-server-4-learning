import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private authService: AuthService, private httpClient: HttpClient) {
    authService.userManager.events.addAccessTokenExpired(_ => console.log("Süre bitti..."));
  }

  message: string;
  bankaData: any = "Bağlantı sağlanamadı...";
   ngOnInit(): void {
    this.authService.userManager.getUser().then(user => {
      //Kullanıcı login olduysa burası tetiklenecek.
      if (user) {
        console.log(user);
        localStorage.setItem("accessToken", user.access_token)
        localStorage.setItem("refreshToken", user.refresh_token)
        this.message = "Giriş başarılı...";
      }
      else
        this.message = "Giriş başarısız...";
    }).then(() => this.httpClient.get("https://localhost:2000/api/banka", {
      headers: { "Authorization": "Bearer " + localStorage.getItem("accessToken") }
    }).subscribe(data => this.bankaData = data));
  }

  login() {
    this.authService.userManager.signinRedirect();
  }

  logout() {
    this.authService.userManager.signoutRedirect();
  }
}
