import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateDebt, DebtResponse } from '../models/debt.model';

@Injectable({
  providedIn: 'root',
})
export class DebtService {
  private apiUrl = 'http://localhost:5267/api/debts';

  constructor(private http: HttpClient) {}

  getAllDebts(): Observable<DebtResponse[]> {
    return this.http.get<DebtResponse[]>(this.apiUrl);
  }

  getDebtById(id: string): Observable<DebtResponse> {
    return this.http.get<DebtResponse>(`${this.apiUrl}/${id}`);
  }

  createDebt(debt: CreateDebt): Observable<DebtResponse> {
    return this.http.post<DebtResponse>(this.apiUrl, debt);
  }
}
