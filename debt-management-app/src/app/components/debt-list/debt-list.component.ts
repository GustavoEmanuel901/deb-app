import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { DebtService } from '../../core/services/debt.service';
import { DebtResponse } from '../../core/models/debt.model';
import { CpfPipe } from '../../core/models/cpf.pipe';

@Component({
  selector: 'app-debt-list',
  templateUrl: './debt-list.component.html',
  styleUrls: ['./debt-list.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, CpfPipe],
})
export class DebtListComponent implements OnInit {
  debts: DebtResponse[] = [];
  loading = false;
  errorMessage = '';
  filteredDebts: DebtResponse[] = [];
  searchTerm = '';

  constructor(
    private debtService: DebtService,
    private cd: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.loadDebts();
  }

  loadDebts() {
    this.loading = true;
    this.errorMessage = '';
    this.debtService
      .getAllDebts()
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cd.detectChanges();
        }),
      )
      .subscribe({
        next: (data) => {
          this.debts = data;
          this.filterDebts();
        },
        error: (error) => {
          console.error('Error loading debts:', error);
          this.errorMessage = 'Erro ao carregar títulos. Tente novamente.';
        },
      });
  }

  filterDebts() {
    if (!this.searchTerm) {
      this.filteredDebts = this.debts;
    } else {
      this.filteredDebts = this.debts.filter(
        (debt) =>
          debt.titleNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          debt.debtorName.toLowerCase().includes(this.searchTerm.toLowerCase()),
      );
    }
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  }

  getDaysLateInfo(days: number): { text: string; color: string; badge: string } {
    if (days === 0) {
      return { text: 'Em dia', color: 'text-green-600', badge: 'bg-green-100 text-green-800' };
    }
    if (days <= 30) {
      return {
        text: `${days} dias`,
        color: 'text-yellow-600',
        badge: 'bg-yellow-100 text-yellow-800',
      };
    }
    return { text: `${days} dias`, color: 'text-red-600', badge: 'bg-red-100 text-red-800' };
  }
}
