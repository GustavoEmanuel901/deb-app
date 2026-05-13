export interface Installment {
  number: number;
  dueDate: Date;
  amount: number;
}

export interface CreateDebt {
  titleNumber: string;
  debtorName: string;
  debtorCpf: string;
  interestRate: number;
  fineRate: number;
  installments: Installment[];
}

export interface DebtResponse {
  id: string;
  titleNumber: string;
  debtorName: string;
  debtorCpf: string;
  installmentsCount: number;
  originalTotal: number;
  daysLate: number;
  updatedTotal: number;
  installments: InstallmentDetail[];
}

export interface InstallmentDetail {
  number: number;
  dueDate: Date;
  amount: number;
  fine?: number;
  interest?: number;
  updatedAmount?: number;
}
