import { InvoiceItem } from "./invoiceItem.model";

export interface Invoice {
    id: string;
    number: number;
    status: string;
    items: InvoiceItem[];
    createdAt: string;
    updatedAt: string | null;
}

export interface AddInvoiceItem {
    productId: string;
    productCode: string;
    quantity: number;
}