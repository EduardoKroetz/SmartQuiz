import { Quiz } from "./Quiz";

export interface Match {
  id: string,
  score: number,
  createdAt: Date,
  status: string,
  quizId: string,
  userId: string,
  reviewed: boolean,
  reviewId: string ,
  quiz: Quiz | null,
  expiresIn: Date,
  remainingTimeInSeconds: number
}