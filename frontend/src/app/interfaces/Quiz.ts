export interface Quiz {
  id: string,
  title: string,
  description: string,
  expires: boolean,
  expiresInSeconds: number,
  theme: string,
  numberOfQuestion: number,
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
  numberOfQuestion: 0,
  difficulty: "easy",
  theme: "",
  userId: "",
};

export interface CreateQuiz {
  title: string,
  description: string,
  theme: string,
  difficulty: string,
  numberOfQuestions: number,
  expires: boolean,
  expiresInSeconds: number
}


export interface CreateQuizErrors {
  titleError: string | null,
  descriptionError: string | null,
  themeError: string | null,
  difficultyError: string | null,
  expiresInSecondsError: string | null
}

export const createQuizErrorDefault = { titleError: null, descriptionError: null, difficultyError: null, expiresInSecondsError: null,  themeError: null };