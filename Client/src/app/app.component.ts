import { Component, inject, Inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,NgFor],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit  {
  
  title = 'DatingApp';
  http  = inject(HttpClient);
  users : any;
  ngOnInit(): void {
    this.http.get("https://localhost:5098/api/User").subscribe({
      next : response => this.users=response,
      error : err=>console.log(err),
      complete : ()=>console.log("completed")
    });
  }
}
