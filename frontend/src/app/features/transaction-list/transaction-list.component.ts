import { Component, Input, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountSummary, Transaction } from '../../core/models/account.model';
import { TransactionTypeLabelPipe } from '../../core/pipes/transaction-type-label.pipe';
import { AmountSignClassPipe } from '../../core/pipes/amount-sign-class.pipe';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule, TransactionTypeLabelPipe, AmountSignClassPipe],
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss']
})
export class TransactionListComponent implements OnChanges {

  @Input() accounts: AccountSummary[] = [];
  @Input() selectedAccountId: string | null = null;

  transactions: Transaction[] = [];

  ngOnChanges(): void {
    const account = this.accounts.find(a => a.id === this.selectedAccountId);
    this.transactions = account?.transactions ?? [];
  }
}
