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
import { QuizComponent } from './pages/quiz/quiz.component';
import { MatchDetailsComponent } from './pages/match-details/match.component';
import { PlayMatchComponent } from './pages/play-match/play-match.component';
import { CreateQuizComponent } from './pages/create-quiz/create-quiz.component';
import { CreateQuizQuestionsComponent } from './pages/create-quiz-questions/create-quiz-questions.component';

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
              { path: "generate-quiz", component: GenerateQuizComponent },
              { path: "create-quiz", component: CreateQuizComponent },
              { path: "create-quiz/:id/questions/:order", component: CreateQuizQuestionsComponent },
              { path: ":id", component: QuizComponent },
            ]
          },
          { path: "matches", children: [
              { path: ":id", component: MatchDetailsComponent },
              { path: "play/:id", component: PlayMatchComponent }
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
