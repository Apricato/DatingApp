import { HttpClient } from '@angular/common/http';
import { Component, signal, inject, OnInit} from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css',
})


export class App implements OnInit {
  private http = inject(HttpClient);
  protected readonly title = signal('Dating App');
  protected members = signal<any>([]);

  async ngOnInit(): Promise<void> {
    this.members.set(await this.getMembers());
  }

  async getMembers(): Promise<Object>  {
    try {
      return lastValueFrom(this.http.get('https://localhost:5001/api/members'));
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
}
