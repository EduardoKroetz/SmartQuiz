import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LayoutNoMainComponent } from './layouts/layout-no-main/layout-no-main.component';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { QuizzesComponent } from './pages/quizzes/quizzes.component';
import { GenerateQuizComponent } from './pages/generate-quiz/generate-quiz.component';
import { HistoryComponent } from './pages/history/history.component';

export const routes: Routes = [
  {
    path: "", component: LayoutNoMainComponent, children: [
      {
        path: "", component: HomeComponent
      }
    ]
  },
  {
    path: "quizzes", component: BaseLayoutComponent, children: [
      {
        path: "", component: QuizzesComponent,
      },
      {
        path: "generate-quiz", component: GenerateQuizComponent 
      }
    ]
  },
  {
    path: "", component: BaseLayoutComponent, children: [
      { path: "history", component: HistoryComponent }
    ]
  }
];
