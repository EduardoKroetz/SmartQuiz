export class ErrorUtils {
  static getErrorMessage(errors: string[], keywords: string[]): string | null {
    return errors.find(error => 
      keywords.some(keyword => error.toLowerCase().includes(keyword.toLowerCase()))
    ) || null;
  }

  static getErrorFromResponse(error: any, defaultMessage = "Não foi possível completar a requisição") : string {
    return error.error.errors[0] ?? defaultMessage;
  }
}