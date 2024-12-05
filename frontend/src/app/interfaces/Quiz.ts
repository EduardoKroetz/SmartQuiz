export interface Quiz {
  id: string,
  title: string,
  description: string,
  expires: boolean,
  expiresInSeconds: number,
  theme: string,
  numberOfQuestions: number,
  difficulty: string,
  userId: string,
  isActive: boolean
}

export const defaultQuiz: Quiz = {
  id: "",
  title: "",
  description: "",
  expires: true,
  expiresInSeconds: 0,
  isActive: true,
  numberOfQuestions: 0,
  difficulty: "easy",
  theme: "",
  userId: "",
};