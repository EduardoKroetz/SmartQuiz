
export class MatchUtils {

  static FormatStatus(status: string) {
    switch(status) 
    {
      case "Created":
        return "Não finalizado"
      case "Finished":
        return "Concluído"
      case "Failed":
        return "Falhou"
      default:
        return status;
    }
  }
}