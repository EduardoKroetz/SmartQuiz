import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Quiz } from '../../interfaces/Quiz';
import { CommonModule } from '@angular/common';
import { QuizCardComponent } from "../../components/quiz-card/quiz-card.component";

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [RouterLink, CommonModule, QuizCardComponent],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.css'
})
export class QuizzesComponent {
  quizzes: Quiz[] = [
    {
      id: '1',
      title: 'Quiz de Matemática',
      description: 'Teste seus conhecimentos em álgebra e geometria.',
      expires: true,
      expiresInSeconds: 3600,
      isActive: true,
      numberOfQuestions: 10,
      theme: 'Matemática',
      userId: 'user123',
    },
    {
      id: '2',
      title: 'Quiz de História',
      description: 'Descubra o quanto você sabe sobre história mundial.',
      expires: false,
      expiresInSeconds: 0,
      isActive: true,
      numberOfQuestions: 15,
      theme: 'História',
      userId: 'user456',
    },
    {
      id: '3',
      title: 'Quiz de Ciências',
      description: 'Desafios sobre biologia, química e física.',
      expires: true,
      expiresInSeconds: 7200,
      isActive: false,
      numberOfQuestions: 20,
      theme: 'Ciências',
      userId: 'user789',
    },
    {
      id: '4',
      title: 'Quiz de Literatura',
      description: 'Teste seus conhecimentos sobre clássicos da literatura.',
      expires: false,
      expiresInSeconds: 0,
      isActive: true,
      numberOfQuestions: 12,
      theme: 'Literatura',
      userId: 'user101',
    },
    {
      id: '5',
      title: 'Quiz de Geografia',
      description: 'Avalie sua memória sobre países, capitais e mapas.',
      expires: true,
      expiresInSeconds: 1800,
      isActive: false,
      numberOfQuestions: 8,
      theme: 'Geografia',
      userId: 'user202',
    },
    {
      id: '6',
      title: 'Quiz de Lógica de Programação',
      description: 'Desafie suas habilidades em algoritmos e estruturas de controle.',
      expires: true,
      expiresInSeconds: 3600,
      isActive: true,
      numberOfQuestions: 12,
      theme: 'Programação',
      userId: 'user001',
    },
    {
      id: '7',
      title: 'Quiz de JavaScript',
      description: 'Teste seus conhecimentos sobre funções, arrays e promessas.',
      expires: true,
      expiresInSeconds: 7200,
      isActive: true,
      numberOfQuestions: 15,
      theme: 'JavaScript',
      userId: 'user002',
    },
    {
      id: '8',
      title: 'Quiz de Banco de Dados',
      description: 'Quanto você sabe sobre SQL, normalização e comandos básicos?',
      expires: false,
      expiresInSeconds: 0,
      isActive: true,
      numberOfQuestions: 20,
      theme: 'Banco de Dados',
      userId: 'user003',
    },
    {
      id: '9',
      title: 'Quiz de C#',
      description: 'Avalie seu conhecimento sobre o uso de LINQ, classes e métodos.',
      expires: true,
      expiresInSeconds: 5400,
      isActive: false,
      numberOfQuestions: 10,
      theme: 'C#',
      userId: 'user004',
    },
    {
      id: '10',
      title: 'Quiz de DevOps',
      description: 'Verifique seus conhecimentos sobre Docker, CI/CD e automação.',
      expires: true,
      expiresInSeconds: 3600,
      isActive: true,
      numberOfQuestions: 8,
      theme: 'DevOps',
      userId: 'user005',
    },
  ];
}