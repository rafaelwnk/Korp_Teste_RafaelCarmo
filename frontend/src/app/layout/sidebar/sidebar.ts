import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  imports: [RouterModule],
  selector: 'app-sidebar',
  templateUrl: './sidebar.html',
})
export class Sidebar {
  constructor(private router: Router) {}
}
