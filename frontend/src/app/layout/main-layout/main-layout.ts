import { Component } from '@angular/core';
import { Sidebar } from "../sidebar/sidebar";
import { RouterOutlet } from '@angular/router';

@Component({
  imports: [Sidebar, RouterOutlet],
  selector: 'app-main-layout',
  template:'<app-sidebar></app-sidebar><router-outlet></router-outlet>'
})
export class MainLayout {}
