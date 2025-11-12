import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AccountSummary, TransferRequest } from '../../core/models/account.model';
import { AtmApiService } from '../../core/services/atm-api.service';

@Component({
  selector: 'app-transfer',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.scss']
})
export class TransferComponent {

  @Input() accounts: AccountSummary[] = [];
  @Output() transferCompleted = new EventEmitter<void>();

  fromAccountId: string | null = null;
  toAccountId: string | null = null;
  amount: number | null = null;

  loading = false;
  error: string | null = null;

  constructor(private atmApi: AtmApiService) {}

  submit(): void {
    if (!this.fromAccountId || !this.toAccountId) {
      this.error = 'Select both source and destination accounts.';
      return;
    }

    if (this.fromAccountId === this.toAccountId) {
      this.error = 'Source and destination must be different.';
      return;
    }

    if (!this.amount || this.amount <= 0) {
      this.error = 'Enter a positive amount.';
      return;
    }

    const payload: TransferRequest = {
      fromAccountId: this.fromAccountId,
      toAccountId: this.toAccountId,
      amount: this.amount
    };

    this.loading = true;
    this.error = null;

    this.atmApi.transfer(payload).subscribe({
      next: () => {
        this.loading = false;
        this.amount = null;
        this.transferCompleted.emit();
      },
      error: err => {
        this.loading = false;
        this.error = err?.error ?? 'Transfer failed.';
        console.error(err);
      }
    });
  }
}
