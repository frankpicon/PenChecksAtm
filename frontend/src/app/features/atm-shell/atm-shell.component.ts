import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountSummary } from '../../core/models/account.model';
import { AtmApiService } from '../../core/services/atm-api.service';
import { AccountListComponent } from '../account-list/account-list.component';
import { TransactionListComponent } from '../transaction-list/transaction-list.component';
import { DepositWithdrawComponent } from '../deposit-withdraw/deposit-withdraw.component';
import { TransferComponent } from '../transfer/transfer.component';

type Tab = 'accounts' | 'depositWithdraw' | 'transfer';

@Component({
  selector: 'atm-shell',
  standalone: true,
  imports: [
    CommonModule,
    AccountListComponent,
    TransactionListComponent,
    DepositWithdrawComponent,
    TransferComponent
  ],
  templateUrl: './atm-shell.component.html',
  styleUrls: ['./atm-shell.component.scss']
})
export class AtmShellComponent implements OnInit {

  loading = false;
  error: string | null = null;
  accounts: AccountSummary[] = [];
  selectedAccountId: string | null = null;
  activeTab: Tab = 'accounts';

  constructor(private atmApi: AtmApiService) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.loading = true;
    this.error = null;

    this.atmApi.getAccounts().subscribe({
      next: accounts => {
        this.accounts = accounts;
        if (!this.selectedAccountId && accounts.length > 0) {
          this.selectedAccountId = accounts[0].id;
        }
        this.loading = false;
      },
      error: err => {
        this.error = 'Failed to load accounts.';
        console.error(err);
        this.loading = false;
      }
    });
  }

  onAccountChanged(accountId: string): void {
    this.selectedAccountId = accountId;
  }

  onRefresh(): void {
    this.loadAccounts();
  }

  setTab(tab: Tab): void {
    this.activeTab = tab;
  }
}
