import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  postUpload(file: File, name: string): any {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, name);
    return this.http.post(`${this.baseURL}upload`, formData);
  }

  postEvento(evento: Evento): Observable<Evento>{
    let str = JSON.stringify(evento , function(k, v): any{
        if (k === 'qtdPessoas'){
          v = parseInt(v);
        }
        return v;
    });
    let ev = JSON.parse(str);
    return this.http.post<Evento>(this.baseURL, ev, {headers: this.tokenHeader});
  }

  putEvento(evento: Evento, id: number): Observable<Evento>{
    let str = JSON.stringify(evento , function(k, v): any{
        if (k === 'qtdPessoas'){
          v = parseInt(v);
        }
        return v;
    });
    let ev = JSON.parse(str);
    return this.http.put<Evento>(`${this.baseURL}${id}`, ev, {headers: this.tokenHeader});
  }

  deleteEvento(id: number): Observable<any>{
    return this.http.delete(`${this.baseURL}${id}`, {headers: this.tokenHeader});
  }

}
