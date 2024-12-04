import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LayoutNoMainComponent } from './layouts/layout-no-main/layout-no-main.component';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { QuizzesComponent } from './pages/quizzes/quizzes.component';

export const routes: Routes = [
  {
    path: "", component: LayoutNoMainComponent, children: [
      {
        path: "", component: HomeComponent
      }
    ],
  },
  {
    path: "", component: BaseLayoutComponent, children: [
      {
        path: "quizzes", component: QuizzesComponent
      }
    ]
  }
];
