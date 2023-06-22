import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Token } from '../models/token';
import { TokenResponse } from '../models/responses/token.response';






@Injectable({
  providedIn: 'root',
})
export class TokenService {
  private readonly apiUrl = `${environment.apiUrl}api/token`;

  constructor(private http: HttpClient) { }
  

  getTokens(pageIndex: number, pageSize: number) {
    return this.http.get<TokenResponse>(`${this.apiUrl}/getTokens?pageIndex=${pageIndex}&pageSize=${pageSize}`)
  }
  addTokens(_name: string, numDateExpired: number){
    return this.http.post(`${this.apiUrl}/addToken?name=${_name}&numDateExpired=${numDateExpired}`, {}, {responseType: 'arraybuffer'})
  }
  deactiveToken(id: string){
    return this.http.put(`${this.apiUrl}/deactiveToken?id=${id}`, {})
  }
}
