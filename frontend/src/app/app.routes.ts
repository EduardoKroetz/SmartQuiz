import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LayoutNoMainComponent } from './layouts/layout-no-main/layout-no-main.component';

export const routes: Routes = [
  {
    path: "", component: LayoutNoMainComponent, children: [
      {
        path: "", component: HomeComponent
      }
    ] 
  }
];
