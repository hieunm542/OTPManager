import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PhoneService } from '../core/services/phone.service';
import { Phone } from '../core/models/phone';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, startWith, switchMap } from 'rxjs';
import { TokenResponse } from '../core/models/responses/token.response';
import { PhoneResponse } from '../core/models/responses/phone.response';
import * as $ from 'jquery';
@Component({
  selector: 'app-phone',
  templateUrl: './phone.component.html',
  styleUrls: ['./phone.component.css']
})
export class PhoneComponent implements OnInit {
  public phoneData: Phone[] = [];
  constructor(private phoneService: PhoneService, private toastr: ToastrService) { }


  public id: string = "";
  public ip: string = "";
  public phoneNumber: string = "";
  public account: string = "";
  public password: string = "";
  public isLoadingResults: boolean = false;

  //table define
  resultsLength: number = 0;
  public displayedColumns: string[] = ['index', 'phoneNumber', 'ip', 'account','status', 'button'];
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
        this.isLoadingResults = true;
        const pageIndex = this.paginator.pageIndex;
        const pageSize = this.paginator.pageSize;
        return this.getPhones(pageIndex, pageSize);
      }),
    )
    .subscribe((data: PhoneResponse) => {
      this.resultsLength = data.total;
      this.phoneData = data.items;
      this.isLoadingResults = false;
    });
  }
  getPhones(pageIndex: number, pageSize: number){
    return this.phoneService.getPhones(pageIndex, pageSize);
  }
  addPhone(){
    console.log(this.ip);
    this.phoneService.addPhone(this.phoneNumber, this.ip, this.account,this.password).subscribe((data: any)=>{
        console.log(data);
        this.renderTable();
        this.toastr.success("Create success");
        $("#phoneAdd .close").click();
    });

    
  }
  deletePhone(id: string){
      this.phoneService.deletePhone(id).subscribe((data: any)=>{
        console.log(data);
        this.renderTable();
        this.toastr.success("Delete success");
      })
  }
  phoneUpdate(){
    let phone: Phone = {
      id: this.id,
      phoneNumber: this.phoneNumber,
      ip: this.ip,
      account: this.account,
      password: this.password,
      connectStatus: false
    };

    this.phoneService.updatePhone(phone).subscribe((data)=>{
      console.log(data);
      
      this.toastr.success("Update success");
      $("#phoneUpdate .close").click();
    })
  }
  viewUpdatePhone(id: string)
  {
    let query = this.phoneData.filter((x)=>{ return x.id == id}).map((data)=>{
      this.id = data.id;
      this.ip = data.ip;
      this.phoneNumber = data.phoneNumber;
      this.account = data.account;
      this.password = data.password;
    });
  }
  removeInput(){
    this.id = '';
    this.ip = '';
    this.phoneNumber = '';
    this.account = '';
    this.password = '';
  }
}
