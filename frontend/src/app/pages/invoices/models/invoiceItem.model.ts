export interface InvoiceItem {
    id: string;
    productId: string;
    productCode: string;
    quantity: number;
    createdAt: string;
    updatedAt: string | null;
}