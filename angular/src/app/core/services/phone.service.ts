import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Phone } from '../models/phone';
import { PhoneResponse } from '../models/responses/phone.response';

@Injectable({
  providedIn: 'root'
})
export class PhoneService {
  private readonly apiUrl = `${environment.apiUrl}api/phone`;

  constructor(private http: HttpClient) { }


  getPhones(pageIndex: number, pageSize: number) {
    return this.http.get<PhoneResponse>(`${this.apiUrl}/getPhones?pageIndex=${pageIndex}&pageSize=${pageSize}`)
  }
  addPhone(phoneNumber: string, ip: string, account: string, password: string) {
    return this.http.post<Phone[]>(`${this.apiUrl}/addPhone`, { phoneNumber,ip, account,password  })
  }
  deletePhone(id:string) {
    return this.http.post(`${this.apiUrl}/deletePhone?id=${id}`, {})
  }
  updatePhone(phone: Phone){
    return this.http.put(`${this.apiUrl}/updatePhone`,phone )
  }
}
