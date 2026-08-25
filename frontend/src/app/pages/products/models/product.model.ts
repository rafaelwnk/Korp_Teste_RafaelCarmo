export interface Product {
    id: string;
    code: string;
    description: string;
    stockBalance: number;
    createdAt: string;
    updatedAt: string | null;
}

export type CreateProduct = Omit<Product, 'id' | 'createdAt' | 'updatedAt'>;

export interface AdjustStock {
    quantity: number;
}

export interface UpdateDescription {
    description: string;
}