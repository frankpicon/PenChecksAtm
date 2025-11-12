import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'transactionTypeLabel',
  standalone: true,
  pure: true
})
export class TransactionTypeLabelPipe implements PipeTransform {
  transform(value: string | null | undefined): string {
    if (!value) {
      return '';
    }

    switch (value) {
      case 'Deposit':
        return 'Deposit';
      case 'Withdrawal':
        return 'Withdrawal';
      case 'TransferIn':
        return 'Transfer In';
      case 'TransferOut':
        return 'Transfer Out';
      default:
        return value.replace(/([a-z])([A-Z])/g, '$1 $2');
    }
  }
}
