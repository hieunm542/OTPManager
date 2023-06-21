import { Component, OnInit, ViewChild } from '@angular/core';
import { TokenService } from '../core/services/token.service';
import { Token } from '../core/models/token';
import { ToastrService } from 'ngx-toastr';
import { TokenResponse } from '../core/models/responses/token.response';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { merge, startWith, switchMap } from 'rxjs';
import * as $ from 'jquery';
@Component({
  selector: 'app-token',
  templateUrl: './token.component.html',
  styleUrls: ['./token.component.css']
})
export class TokenComponent implements OnInit {
  @ViewChild('tokenAdd')
  public tokenData: Token[] = [];
  public currentToken: string = "";
  public _name: string = "";
  public numDateExpired: number = 30;
  constructor(private tokenService: TokenService, private toastr: ToastrService) { }
  resultsLength: number = 0;
  public displayedColumns: string[] = ['index', 'name', 'tokenString', 'numberDateExpired','createdAt', 'button'];
  ngOnInit(): void {

  }
  @ViewChild(MatPaginator)
  paginator!: MatPaginator;
  ngAfterViewInit(): void {

    this.renderTable();
  }
  renderTable(){
    merge(this.paginator.page)
    .pipe(
      startWith({} as PageEvent),
      switchMap((pageEvent: PageEvent) => {
        console.log(pageEvent);
        
        const pageIndex = this.paginator.pageIndex;
        const pageSize = this.paginator.pageSize;
        return this.getToken(pageIndex, pageSize);
      }),
    )
    .subscribe((data: TokenResponse) => {
      this.resultsLength = data.total;
      this.tokenData = data.items;
    });
  }
  clickView(current: string){
    this.currentToken = current;
  }
  copyToken(){
    navigator.clipboard.writeText(this.currentToken);
    this.toastr.success("Copy token to clipboard!")
  }
  getToken(pageIndex: number, pageSize: number){
    return this.tokenService.getTokens(pageIndex, pageSize);
  }
  addToken(){
    this.tokenService.addTokens(this._name, this.numDateExpired)
    .subscribe(
    ()=>{
      
      this.toastr.success("Create Success");
      $("#tokenAdd .close").click();
      this.renderTable();
    }, 
    (error)=>{
      this.toastr.error(error);
      this.renderTable();
    });
  }

}
