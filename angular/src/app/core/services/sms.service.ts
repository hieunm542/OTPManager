import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SMS } from '../models/sms';
import { SMSResponse } from '../models/responses/sms.response';
import { SMSRequest } from '../models/request/sms.request';

@Injectable({
  providedIn: 'root'
})
export class SMSService {
  private readonly apiUrl = `${environment.apiUrl}api/sms`;

  constructor(private http: HttpClient) { }


  getSMS(smsRequest: SMSRequest) {
    return this.http.post<SMSResponse>(`${this.apiUrl}/getSMS`, smsRequest)
  }
}