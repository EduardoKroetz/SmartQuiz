export class ErrorUtils {
  static getErrorMessage(errors: string[], keywords: string[]): string | null {
    return errors.find(error => 
      keywords.some(keyword => error.toLowerCase().includes(keyword.toLowerCase()))
    ) || null;
  }

}