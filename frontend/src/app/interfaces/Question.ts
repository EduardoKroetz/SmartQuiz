import { AnswerOption } from "./AnswerOption";

export interface Question {
  id: string,
  text: string,
  quizId: string,
  order: number,
  options: AnswerOption[] | null
}