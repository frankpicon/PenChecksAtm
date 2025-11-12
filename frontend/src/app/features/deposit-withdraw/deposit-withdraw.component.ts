import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AtmApiService } from '../../core/services/atm-api.service';

@Component({
  selector: 'app-deposit-withdraw',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './deposit-withdraw.component.html',
  styleUrls: ['./deposit-withdraw.component.scss']
})
export class DepositWithdrawComponent {

  @Input() accountId: string | null = null;
  @Output() operationCompleted = new EventEmitter<void>();

  mode: 'deposit' | 'withdraw' = 'deposit';
  amount: number | null = null;
  loading = false;
  error: string | null = null;

  constructor(private atmApi: AtmApiService) {}

  setMode(mode: 'deposit' | 'withdraw'): void {
    this.mode = mode;
    this.error = null;
  }

  submit(): void {
    if (!this.accountId || !this.amount || this.amount <= 0) {
      this.error = 'Please enter a positive amount.';
      return;
    }

    this.loading = true;
    this.error = null;

    const obs =
      this.mode === 'deposit'
        ? this.atmApi.deposit(this.accountId, this.amount)
        : this.atmApi.withdraw(this.accountId, this.amount);

    obs.subscribe({
      next: () => {
        this.loading = false;
        this.amount = null;
        this.operationCompleted.emit();
      },
      error: err => {
        this.loading = false;
        this.error = err?.error ?? 'Operation failed.';
        console.error(err);
      }
    });
  }
}
