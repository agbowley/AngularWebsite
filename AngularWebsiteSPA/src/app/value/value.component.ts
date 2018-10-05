import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(x => { // httpclient.get must be suscribed to as it is an observable
      this.values = x; // if a value is returned from the get, set values to it
    }, error => { // if an error is returned from the server, log the error in the console
      console.log(error);
    });
  }

}
