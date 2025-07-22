
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { CreateSaleDTO, Sale, SaleListDTO } from '../models/sale.model';
import { BaseService } from './base.service';

interface ApiResponse<T> {
  data: T;
  success: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class SalesService extends BaseService<Sale> {
  constructor(private http: HttpClient) {
    super(http);
  }

  getSales(): Observable<SaleListDTO[]> {
    return this.http.get<ApiResponse<SaleListDTO[]>>(`${this.apiUrl}/api/sales`)
      .pipe(map(response => response.data));
  }

  getSaleById(id: string): Observable<Sale> {
    return this.http.get<ApiResponse<Sale>>(`${this.apiUrl}/api/sales/${id}`)
      .pipe(map(response => response.data));
  }

  createSale(saleData: CreateSaleDTO): Observable<any> {
    return this.http.post(`${this.apiUrl}/api/sales`, saleData);
  }

  updateSale(id: string, saleData: { customer: string, branch: string }): Observable<any> {
    return this.http.put(`${this.apiUrl}/api/sales/${id}`, saleData);
  }
}
