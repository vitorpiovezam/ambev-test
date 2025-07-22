
import { Component, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { toSignal } from '@angular/core/rxjs-interop';

// Angular Material Imports
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { SalesService } from '../../../shared/services/sales.service';

@Component({
  selector: 'app-edit',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressBarModule
  ],
  template: `
    <div class="container">
      <mat-card>
        <mat-card-title>Editar Venda</mat-card-title>
        @if (sale()) {
          <form [formGroup]="saleForm" (ngSubmit)="onSubmit()">
            <mat-card-content>
              <mat-form-field appearance="outline">
                <mat-label>Cliente</mat-label>
                <input matInput formControlName="customer">
              </mat-form-field>

              <mat-form-field appearance="outline">
                <mat-label>Filial</mat-label>
                <input matInput formControlName="branch">
              </mat-form-field>
            </mat-card-content>
            <mat-card-actions>
              <button mat-raised-button color="primary" type="submit" [disabled]="saleForm.invalid || isSaving()">
                {{ isSaving() ? 'Salvando...' : 'Salvar' }}
              </button>
              <a mat-stroked-button routerLink="/sales/list">Cancelar</a>
            </mat-card-actions>
          </form>
        } @else {
          <mat-progress-bar mode="indeterminate"></mat-progress-bar>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .container
      padding: 2rem;
      max-width: 600px;
      margin: auto;


    form
      display: flex;
      flex-direction: column;

    mat-form-field
      width: 100%;


    mat-card-actions
      justify-content: flex-end;
  
  `]
})
export class EditComponent {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private salesService = inject(SalesService);
  private fb = inject(FormBuilder);

  private saleId = signal(this.route.snapshot.paramMap.get('id')!);
  sale = toSignal(this.salesService.getSaleById(this.saleId()));
  isSaving = signal(false);

  saleForm: FormGroup = this.fb.group({
    customer: ['', Validators.required],
    branch: ['', Validators.required]
  });

  constructor() {
    effect(() => {
      const currentSale = this.sale();
      if (currentSale) {
        this.saleForm.patchValue({
          customer: currentSale.customer,
          branch: currentSale.branch
        });
      }
    });
  }

  onSubmit() {
    if (this.saleForm.invalid) {
      return;
    }
    this.isSaving.set(true);
    this.salesService.updateSale(this.saleId(), this.saleForm.value).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.router.navigate(['/sales/list']);
      },
      error: (err) => {
        console.error('Erro ao atualizar venda', err);
        this.isSaving.set(false);
      }
    });
  }
}
