
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, FormArray, ReactiveFormsModule, Validators } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { SalesService } from '../../../shared/services/sales.service';
import { CreateSaleDTO } from '../../../shared/models/sale.model';

@Component({
    selector: 'app-new',
    standalone: true,
    imports: [
        CommonModule,
        RouterModule,
        ReactiveFormsModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatIconModule,
        MatSnackBarModule
    ],
    template: `

    <div class="new-sale-container">
        <mat-card>
            <mat-card-title>Criar Nova Venda</mat-card-title>
            <form [formGroup]="saleForm" (ngSubmit)="onSubmit()">
                <mat-card-content>
                    <div class="form-row">
                        <mat-form-field appearance="outline">
                            <mat-label>Cliente</mat-label>
                            <input matInput formControlName="customer" required>
                        </mat-form-field>

                        <mat-form-field appearance="outline">
                            <mat-label>Filial</mat-label>
                            <input matInput formControlName="branch" required>
                        </mat-form-field>
                    </div>

                    <h3>Itens da Venda</h3>

                    <div formArrayName="items">
                        @for (item of items.controls; track $index) {

                            <div [formGroupName]="$index" class="item-row">
                                <mat-form-field appearance="outline" class="item-product">
                                    <mat-label>Produto</mat-label>
                                    <input matInput formControlName="product" required>
                                </mat-form-field>

                                <mat-form-field appearance="outline" class="item-quantity">
                                    <mat-label>Qtd.</mat-label>
                                    <input matInput type="number" formControlName="quantity" required min="1" max="20">
                                </mat-form-field>

                                <mat-form-field appearance="outline" class="item-price">
                                    <mat-label>Preço Unit.</mat-label>
                                    <input matInput type="number" formControlName="unitPrice" required min="0.01">
                                </mat-form-field>

                                <button mat-icon-button color="warn" type="button" (click)="removeItem($index)" [disabled]="items.length === 1" title="Remover Item">
                                    <mat-icon>remove_circle</mat-icon>
                                </button>
                            </div>

                        }
                    </div>

                    <button mat-stroked-button color="accent" type="button" (click)="addItem()">
                        <mat-icon>add</mat-icon> Adicionar Item
                    </button>
                </mat-card-content>

                <mat-card-actions align="end">
                    <a mat-button routerLink="/sales/list">Cancelar</a>
                    <button mat-raised-button color="primary" type="submit" [disabled]="saleForm.invalid">
                    Salvar Venda
                    </button>
                </mat-card-actions>
            </form>
        </mat-card>
    </div>
  `,
    styleUrl: './new.component.sass'
})
export class NewComponent {
    private fb = inject(FormBuilder);
    private salesService = inject(SalesService);
    private router = inject(Router);
    private snackBar = inject(MatSnackBar);

    saleForm: FormGroup;

    constructor() {
        this.saleForm = this.fb.group({
            customer: ['', Validators.required],
            branch: ['', Validators.required],
            items: this.fb.array([this.createItemFormGroup()])
        });
    }

    get items(): FormArray {
        return this.saleForm.get('items') as FormArray;
    }

    createItemFormGroup(): FormGroup {
        return this.fb.group({
            product: ['', Validators.required],
            quantity: [1, [Validators.required, Validators.min(1), Validators.max(20)]],
            unitPrice: [0, [Validators.required, Validators.min(0.01)]]
        });
    }

    addItem(): void {
        this.items.push(this.createItemFormGroup());
    }

    removeItem(index: number): void {
        if (this.items.length > 1) {
            this.items.removeAt(index);
        }
    }

    onSubmit(): void {
        if (this.saleForm.invalid) {
            this.snackBar.open('Por favor, preencha todos os campos corretamente.', 'Fechar', { duration: 3000 });
            return;
        }

        const saleData: CreateSaleDTO = this.saleForm.value;
        this.salesService.createSale(saleData).subscribe({
            next: () => {
                this.snackBar.open('Venda criada com sucesso!', 'OK', { duration: 3000 });
                this.router.navigate(['/sales/list']);
            },
            error: (err) => {
                console.error('Erro ao criar venda', err);
                this.snackBar.open(`Erro: ${err.error?.message || 'Não foi possível criar a venda.'}`, 'Fechar', { duration: 5000 });
            }
        });
    }
}