
import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SaleListDTO } from '../../../shared/models/sale.model';
import { SalesService } from '../../../shared/services/sales.service';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatProgressBarModule,
    MatIconModule,
    MatButtonModule
  ],
  template: `
    <div class="container">
      <header>
        <h1>Lista de Vendas</h1>
        <button mat-raised-button color="primary" routerLink="/sales/new">
          <mat-icon>add</mat-icon>
          Nova Venda
        </button>
      </header>

      @if (isLoading()) {
        <mat-progress-bar mode="indeterminate"></mat-progress-bar>
      }

      @if (sales().length > 0 && !isLoading()) {
        <table mat-table [dataSource]="sales()" class="mat-elevation-z8">
          <ng-container matColumnDef="customer">
            <th mat-header-cell *matHeaderCellDef> Cliente </th>
            <td mat-cell *matCellDef="let sale"> {{sale.customer}} </td>
          </ng-container>

          <ng-container matColumnDef="branch">
            <th mat-header-cell *matHeaderCellDef> Filial </th>
            <td mat-cell *matCellDef="let sale"> {{sale.branch}} </td>
          </ng-container>

          <ng-container matColumnDef="totalAmount">
            <th mat-header-cell *matHeaderCellDef> Valor Total </th>
            <td mat-cell *matCellDef="let sale"> {{sale.totalAmount | currency:'BRL'}} </td>
          </ng-container>

          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef> Status </th>
            <td mat-cell *matCellDef="let sale">
                <span [class.cancelled]="sale.isCancelled">
                    {{ sale.isCancelled ? 'Cancelada' : 'Ativa' }}
                </span>
            </td>
          </ng-container>

          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef> Ações </th>
            <td mat-cell *matCellDef="let sale">
              <button mat-icon-button color="primary" (click)="viewSale(sale.id)" title="Visualizar Venda">
                <mat-icon>visibility</mat-icon>
              </button>
              <button mat-icon-button color="accent" (click)="editSale(sale.id)" [disabled]="sale.isCancelled" title="Editar Venda">
                <mat-icon>edit</mat-icon>
              </button>
            </td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
          <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
        </table>
      } 
  
      @else if (sales().length === 0 && !isLoading()) {
        <div class="empty-state">
          <p>Nenhuma venda encontrada.</p>
          <span>Clique em "Nova Venda" para começar.</span>
        </div>
      }
    </div>
  `,
  styles: [`
    .container
      padding: 2rem;

    table
      width: 100%;

    .cancelled
      color: red;
      text-decoration: line-through;

  `]
})
export class ListComponent {
  private router = inject(Router);
  private salesService = inject(SalesService);

  sales = signal<SaleListDTO[]>([]);
  isLoading = signal(true);
  displayedColumns: string[] = ['customer', 'branch', 'totalAmount', 'status', 'actions'];

  constructor() {
    this.loadSales();
  }

  loadSales() {
    this.isLoading.set(true);
    this.salesService.getSales().subscribe({
      next: (data) => {
        this.sales.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Erro ao carregar vendas', err);
        this.isLoading.set(false);
      }
    });
  }

  viewSale(id: string) {
    this.router.navigate(['/sales/view', id]);
  }

  editSale(id: string) {
    this.router.navigate(['/sales/edit', id]);
  }
}
