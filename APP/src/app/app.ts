import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { response } from 'express';
import { environment } from '../environments/environment';
import { lastValueFrom } from 'rxjs';


@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{
  private http = inject(HttpClient);
  protected readonly title = signal('Dating App')
  protected members = signal<any>([]);
  
  async ngOnInit(): Promise<void> {
    this.members.set(await this.getMembers())
  }

  async getMembers(): Promise<Object>{
    try{
      return lastValueFrom(this.http.get(`${environment.apiUrl}/api/members`))
    }
    catch(error){
      console.error(error);
      throw error;
    }
  }
}
