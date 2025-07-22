import { BaseModel } from "./base.model";

export interface SaleItem extends BaseModel {
  product: string;
  quantity: number;
  unitPrice: number;
  discount: number;
  totalItemAmount: number;
}

export interface Sale extends BaseModel {
  saleDate: Date;
  customer: string;
  branch: string;
  totalAmount: number;
  isCancelled: boolean;
  items: SaleItem[];
}

export interface SaleListDTO {
    id: string;
    saleDate: Date;
    customer: string;
    branch: string;
    totalAmount: number;
    isCancelled: boolean;
    numberOfItems: number;
}

export interface CreateSaleDTO {
  customer: string;
  branch: string;
  items: {
    product: string;
    quantity: number;
    unitPrice: number;
  }[];
}
