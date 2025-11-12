import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { Observable } from 'rxjs';
import { AccountSummary, TransferRequest } from '../models/account.model';

@Injectable({
  providedIn: 'root'
})
export class AtmApiService {

  private readonly baseUrl = `${environment.apiBaseUrl}/accounts`;

  constructor(private http: HttpClient) {}

  getAccounts(): Observable<AccountSummary[]> {
    return this.http.get<AccountSummary[]>(this.baseUrl);
  }

  getAccount(id: string): Observable<AccountSummary> {
    return this.http.get<AccountSummary>(`${this.baseUrl}/${id}`);
  }

  deposit(id: string, amount: number): Observable<AccountSummary> {
    return this.http.post<AccountSummary>(`${this.baseUrl}/${id}/deposit`, { amount });
  }

  withdraw(id: string, amount: number): Observable<AccountSummary> {
    return this.http.post<AccountSummary>(`${this.baseUrl}/${id}/withdraw`, { amount });
  }

  transfer(request: TransferRequest): Observable<{ from: AccountSummary; to: AccountSummary }> {
    return this.http.post<{ from: AccountSummary; to: AccountSummary }>(
      `${this.baseUrl}/transfer`,
      request
    );
  }
}
