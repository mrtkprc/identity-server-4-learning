import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CallbackComponent } from './components/callback/callback.component';
import { HomeComponent } from './components/home/home.component';
import { SilentCallbackComponent } from './components/silent-callback/silent-callback.component';

const routes: Routes = [
  {
    path: '', component: HomeComponent
  },
  {
    path: "callback", component: CallbackComponent
  },
  {
    path: "silent-callback", component: SilentCallbackComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
