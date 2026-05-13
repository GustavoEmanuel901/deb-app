import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'cpf', standalone: true })
export class CpfPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return '';
    const cleaned = value.replace(/\D/g, '');
    if (cleaned.length !== 11) return value;
    return cleaned.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }
}
