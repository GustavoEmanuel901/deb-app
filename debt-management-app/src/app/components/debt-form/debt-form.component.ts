import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DebtService } from '../../core/services/debt.service';

@Component({
  selector: 'app-debt-form',
  templateUrl: './debt-form.component.html',
  styleUrls: ['./debt-form.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class DebtFormComponent {
  debtForm: FormGroup;
  showSuccessMessage = false;
  showErrorMessage = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private debtService: DebtService,
    private router: Router,
  ) {
    this.debtForm = this.fb.group({
      titleNumber: ['', [Validators.required, Validators.maxLength(50)]],
      debtorName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
      debtorCpf: ['', [Validators.required, this.cpfValidator]],
      interestRate: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      fineRate: ['', [Validators.required, Validators.min(0), Validators.max(100)]],
      installments: this.fb.array([], Validators.required),
    });

    this.addInstallment();
  }

  get installments(): FormArray {
    return this.debtForm.get('installments') as FormArray;
  }

  formatCPF(event: any) {
    let value = event.target.value.replace(/\D/g, '');
    if (value.length > 11) value = value.slice(0, 11);

    if (value.length <= 11) {
      if (value.length > 9) {
        value = value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
      } else if (value.length > 6) {
        value = value.replace(/(\d{3})(\d{3})(\d{3})/, '$1.$2.$3');
      } else if (value.length > 3) {
        value = value.replace(/(\d{3})(\d{3})/, '$1.$2');
      } else if (value.length > 0) {
        value = value.replace(/(\d{3})/, '$1');
      }
    }

    this.debtForm.get('debtorCpf')?.setValue(value, { emitEvent: false });
  }

  cpfValidator(control: AbstractControl): ValidationErrors | null {
    const cpf = control.value;
    if (!cpf) return null;

    const cleanCpf = cpf.replace(/\D/g, '');

    if (cleanCpf.length !== 11) {
      return { invalidCpf: true };
    }

    let sum = 0;
    let remainder;

    for (let i = 1; i <= 9; i++) {
      sum += parseInt(cleanCpf.substring(i - 1, i)) * (11 - i);
    }

    remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) remainder = 0;
    if (remainder !== parseInt(cleanCpf.substring(9, 10))) {
      return { invalidCpf: true };
    }

    sum = 0;
    for (let i = 1; i <= 10; i++) {
      sum += parseInt(cleanCpf.substring(i - 1, i)) * (12 - i);
    }

    remainder = (sum * 10) % 11;
    if (remainder === 10 || remainder === 11) remainder = 0;
    if (remainder !== parseInt(cleanCpf.substring(10, 11))) {
      return { invalidCpf: true };
    }

    return null;
  }

  addInstallment() {
    const installmentForm = this.fb.group({
      number: [this.installments.length + 1, [Validators.required, Validators.min(1)]],
      dueDate: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0.01)]],
    });
    this.installments.push(installmentForm);
  }

  removeInstallment(index: number) {
    if (this.installments.length > 1) {
      this.installments.removeAt(index);
      this.reorderInstallmentNumbers();
    }
  }

  reorderInstallmentNumbers() {
    this.installments.controls.forEach((control, index) => {
      control.patchValue({ number: index + 1 });
    });
  }

  onSubmit() {
    if (this.debtForm.valid) {
      const formData = this.debtForm.value;
      const cleanCpf = formData.debtorCpf.replace(/\D/g, '');
      formData.debtorCpf = cleanCpf;

      formData.installments = formData.installments.map((installment: any) => ({
        ...installment,
        dueDate: new Date(installment.dueDate).toISOString(),
      }));

      this.debtService.createDebt(formData).subscribe({
        next: (result) => {
          this.showSuccessMessage = true;
          setTimeout(() => {
            this.router.navigate(['/debts']);
          }, 1500);
        },
        error: (error) => {
          this.showErrorMessage = true;
          this.errorMessage = 'Erro ao cadastrar dívida. Tente novamente.';
          setTimeout(() => {
            this.showErrorMessage = false;
          }, 3000);
        },
      });
    } else {
      Object.keys(this.debtForm.controls).forEach((key) => {
        const control = this.debtForm.get(key);
        control?.markAsTouched();
      });
    }
  }

  getTotalValue(): number {
    let total = 0;
    this.installments.controls.forEach((control) => {
      total += control.get('amount')?.value || 0;
    });
    return total;
  }
}
