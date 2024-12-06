import { format } from "date-fns";
import { ptBR } from "date-fns/locale";

export class DateUtils {
  static FormatDate(date: Date) {
    return format(date, 'dd/MM/yyyy hh:mm:ss', { locale: ptBR })
  }
}
