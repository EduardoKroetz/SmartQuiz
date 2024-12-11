
export default interface Account {
  id: string,
  username: string,
  email: string,
  totalScore: number,
  maxScore: number,
  matchesPlayed: number,
  createdQuizzes: number
}