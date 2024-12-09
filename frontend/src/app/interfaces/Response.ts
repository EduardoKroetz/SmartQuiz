import { AnswerOption } from "./AnswerOption";

export interface Response {
  id: string,
  answerOptionId: string,
  answerOption: AnswerOption,
  correctOption: AnswerOption,
  isCorrect: boolean
}