export type TransactionType = 'Deposit' | 'Withdrawal' | 'TransferIn' | 'TransferOut';

export interface Transaction {
  id: string;
  timestamp: string;
  type: TransactionType;
  amount: number;
  balanceAfter: number;
  description?: string;
}

export interface AccountSummary {
  id: string;
  name: string;
  balance: number;
  transactions: Transaction[];
}

export interface AmountRequest {
  amount: number;
}

export interface TransferRequest {
  fromAccountId: string;
  toAccountId: string;
  amount: number;
}
