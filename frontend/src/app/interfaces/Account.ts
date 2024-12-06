
export default interface Account {
  id: string,
  username: string,
  email: string,
  emailIsVerified: boolean,
  totalScore: number,
  maxScore: number,
  matchesPlayed: number,
  createdQuizzes: number
}