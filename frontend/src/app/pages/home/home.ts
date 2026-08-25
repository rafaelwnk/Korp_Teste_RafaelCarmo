import { Component } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-home',
  templateUrl: './home.html',
})
export class Home {
  socialLinks = [
    { name: 'LinkedIn', icon: 'linkedin', url: 'https://www.linkedin.com/in/rafaelnascimentownk/' },
    { name: 'GitHub', icon: 'github', url: 'https://github.com/rafaelwnk' }
  ];
}
