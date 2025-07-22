
import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';

// Angular Material Imports
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { SalesService } from '../../../shared/services/sales.service';

@Component({
  selector: 'app-view',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatListModule,
    MatProgressBarModule,
    MatChipsModule,
    MatIconModule,
    MatButtonModule
  ],
  template: `
    <div class="container">
      @if (sale()) {
        <mat-card>
          <mat-card-header>
            <mat-card-title>Detalhes da Venda</mat-card-title>
            <mat-card-subtitle>Cliente: {{ sale()?.customer }}</mat-card-subtitle>
          </mat-card-header>
          <mat-card-content>
            <p><strong>Filial:</strong> {{ sale()?.branch }}</p>
            <p><strong>Data:</strong> {{ sale()?.saleDate | date:'dd/MM/yyyy HH:mm' }}</p>
            <p><strong>Valor Total:</strong> {{ sale()?.totalAmount | currency:'BRL' }}</p>
            <mat-chip-listbox>
                <mat-chip [highlighted]="true" [color]="sale()?.isCancelled ? 'warn' : 'primary'">
                    {{ sale()?.isCancelled ? 'Cancelada' : 'Ativa' }}
                </mat-chip>
            </mat-chip-listbox>

            <h3>Itens da Venda</h3>
            <mat-list>
              @for (item of sale()?.items; track item.id) {
                <mat-list-item>
                  <span matListItemTitle>{{ item.product }}</span>
                  <span matListItemLine>
                    {{ item.quantity }}x {{ item.unitPrice | currency:'BRL' }} |
                    Desconto: {{ item.discount | currency:'BRL' }} |
                    Total: {{ item.totalItemAmount | currency:'BRL' }}
                  </span>
                </mat-list-item>
              }
            </mat-list>
          </mat-card-content>
          <mat-card-actions>
            <a mat-stroked-button routerLink="/sales/list">Voltar para a Lista</a>
          </mat-card-actions>
        </mat-card>
      } @else {
        <mat-progress-bar mode="indeterminate"></mat-progress-bar>
      }
    </div>
  `,
  styles: [`
    .container
      padding: 2rem;
      max-width: 800px;
      margin: auto;


    mat-card-content p
      margin-bottom: 1rem;


    h3
      margin-top: 2rem;

  `]
})
export class ViewComponent {
  private route = inject(ActivatedRoute);
  private salesService = inject(SalesService);

  private saleId = signal(this.route.snapshot.paramMap.get('id')!);
  sale = toSignal(this.salesService.getSaleById(this.saleId()));
}
