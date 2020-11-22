import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  baseURL = 'http://localhost:5000/api/Evento/';

  constructor(private http: HttpClient) { }

  getAllEvento(): Observable<Evento[]>{
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventoById(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseURL}${id}`);
  }

  getEventoByTema(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
  }

  postEvento(evento: Evento): Observable<Evento>{
    let str = JSON.stringify(evento , function(k, v): any{
        if (k === 'qtdPessoas'){
          v = parseInt(v);
        }
        return v;
    });
    let ev = JSON.parse(str);
    return this.http.post<Evento>(this.baseURL, ev);
  }

  putEvento(evento: Evento, id: number): Observable<Evento>{
    let str = JSON.stringify(evento , function(k, v): any{
        if (k === 'qtdPessoas'){
          v = parseInt(v);
        }
        return v;
    });
    let ev = JSON.parse(str);
    return this.http.put<Evento>(`${this.baseURL}${id}`, ev);
  }

  deleteEvento(id: number): Observable<any>{
    return this.http.delete(`${this.baseURL}${id}`);
  }

}
