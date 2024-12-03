import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';

export const routes: Routes = [
  {
    path: "", component: BaseLayoutComponent, children: [
      {
        path: "", component: HomeComponent
      }
    ] 
  }
];
