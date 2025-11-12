import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'amountSignClass',
  standalone: true,
  pure: true
})
export class AmountSignClassPipe implements PipeTransform {
  transform(value: number | null | undefined): string {
    if (value == null) {
      return '';
    }

    if (value > 0) {
      return 'amount--positive';
    } else if (value < 0) {
      return 'amount--negative';
    }

    return 'amount--neutral';
  }
}
