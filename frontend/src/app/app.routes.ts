import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { BaseLayoutComponent } from './layouts/base-layout/base-layout.component';
import { QuizzesComponent } from './pages/quizzes/quizzes.component';
import { GenerateQuizComponent } from './pages/generate-quiz/generate-quiz.component';
import { HistoryComponent } from './pages/history/history.component';
import { AccountComponent } from './pages/account/account.component';
import { BaseLayoutMainComponent } from './layouts/base-layout-main/base-layout-main.component';
import { BaseLayoutMainTransparentComponent } from './layouts/base-layout-main-transparent/base-layout-main-transparent.component';
import { RegisterComponent } from './pages/register/register.component';
import { LoginComponent } from './pages/login/login.component';

export const routes: Routes = [
  {
    path: "", component: BaseLayoutComponent, children: [

      { 
        path: "", component: BaseLayoutMainTransparentComponent, children:
        [
          { path: "", component: HomeComponent }
        ] 
      },

      { 
        path: "", component: BaseLayoutMainComponent, children: 
        [
          {
            path: "quizzes", children:
            [
              { path: "", component: QuizzesComponent },
              { path: "generate-quiz", component: GenerateQuizComponent }
            ]
          },
          { path: "history", component: HistoryComponent },
          { path: "account", component: AccountComponent }
        ]
      },
    ]
  },
  { path:  "register", component: RegisterComponent },
  { path:  "login", component: LoginComponent }
];
