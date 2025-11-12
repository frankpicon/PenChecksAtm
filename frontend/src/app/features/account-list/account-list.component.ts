import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountSummary } from '../../core/models/account.model';

@Component({
  selector: 'app-account-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './account-list.component.html',
  styleUrls: ['./account-list.component.scss']
})
export class AccountListComponent {
  @Input() accounts: AccountSummary[] = [];
  @Input() selectedAccountId: string | null = null;
  @Output() accountChanged = new EventEmitter<string>();

  onSelect(id: string): void {
    this.accountChanged.emit(id);
  }
}
