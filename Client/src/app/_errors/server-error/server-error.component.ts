import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [NgFor,NgIf],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css'
})
export class ServerErrorComponent {
  error:any;
  constructor(private router : Router)
  {
    const nav= this.router.getCurrentNavigation();
    this.error=nav?.extras.state?.["error"];
  }
}
