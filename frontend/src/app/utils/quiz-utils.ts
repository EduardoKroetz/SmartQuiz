
export class QuizUtils {

  static FormatDifficulty(difficulty: string) {
    switch (difficulty) {
      case 'Easy':
        return 'Fácil'
      case 'Medium':
        return 'Médio'
      case 'Hard':
        return 'Difícil'
      default:
        return ''
    }
  }
}