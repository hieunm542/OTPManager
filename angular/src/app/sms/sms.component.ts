import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { SMSService } from '../core/services/sms.service';
import { SMS } from '../core/models/sms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge } from 'rxjs';
import { map, startWith, switchMap } from 'rxjs/operators';
import { SMSResponse } from '../core/models/responses/sms.response';
import { SMSRequest } from '../core/models/request/sms.request';
import { FormGroup, FormControl } from '@angular/forms';
@Component({
  selector: 'app-sms',
  templateUrl: './sms.component.html',
  styleUrls: ['./sms.component.css']
})
export class SmsComponent implements OnInit, AfterViewInit {
  resultsLength: number = 0;
  constructor(private smsService: SMSService) { }
  public displayedColumns: string[] = ['index', 'from', 'content', 'receivedTime', 'phoneNumber', 'button'];
  smsData: SMS[] = [];
  smsView: SMS = {
    id: '',
    index: '',
    from: '',
    content: '',
    receivedTime: '',
    phoneID: '',
    phoneNumber: ''
  };
  formFilter = new FormGroup({
    from: new FormControl(),
    content: new FormControl(),
    fromDate: new FormControl(),
    toDate: new FormControl(),
    phoneNumber: new FormControl()
  });
  
  @ViewChild(MatPaginator)
  paginator!: MatPaginator;
  ngOnInit(): void {
    
  }
  ngAfterViewInit(): void {
    this.renderTable();
    
  }
  renderTable(){
    merge(this.paginator.page)
    .pipe(
      startWith({} as PageEvent),
      switchMap((pageEvent: PageEvent) => {
        console.log(pageEvent);
        return this.getSMS();
      }),
    )
    .subscribe((data: SMSResponse) => {
      this.resultsLength = data.total;
      this.smsData = data.items;
    });
  }
  filter(){
    this.renderTable();
    console.log(this.formFilter);
    
  }
  getSMS(){
    const smsRequest: SMSRequest = {
      pageIndex: this.paginator.pageIndex,
      pageSize: this.paginator.pageSize,
      from: this.formFilter.value.from,
      content: this.formFilter.value.content,
      maxReceivedTime: this.formFilter.value.toDate,
      minReceivedTime: this.formFilter.value.fromDate,
      phoneNumber: this.formFilter.value.phoneNumber,
    }
    return this.smsService.getSMS(smsRequest)
  }
  clickView(sms: SMS){
    console.log(sms);
    this.smsView = sms;
  }
}
