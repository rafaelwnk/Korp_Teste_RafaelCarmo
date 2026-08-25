import { Routes } from '@angular/router';
import { MainLayout } from './layout/main-layout/main-layout';

export const routes: Routes = [
    {
        path: '',
        component: MainLayout,
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/home/home').then(m => m.Home)
            },
            {
                path: 'products',
                loadComponent: () => import('./pages/products/products').then(m => m.Products)
            },
            {
                path: 'invoices',
                loadComponent: () => import('./pages/invoices/invoices').then(m => m.Invoices)
            }
        ]
    }
];
