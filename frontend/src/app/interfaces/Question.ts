import { AnswerOption } from "./AnswerOption";

export interface Question {
  id: string,
  text: string,
  quizId: string,
  order: number,
  options: AnswerOption[] | null
}

export interface CreateQuestion {
  text: string,
  quizId: string,
  order: number,
  options: { response: string, isCorrectOption: boolean }[]
}

export const createQuestionDefault: CreateQuestion = {
  options: [
    { response: "", isCorrectOption: false },
    { response: "", isCorrectOption: false },
    { response: "", isCorrectOption: false },
    { response: "", isCorrectOption: false }
  ],
  order: 0,
  quizId: '',
  text: ''
}
