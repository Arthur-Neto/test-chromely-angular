import { Component, OnInit } from '@angular/core';
import axios from 'axios';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {
  public forecasts: WeatherForecast[];

  constructor() {
    this.forecasts = [];
  }

  ngOnInit(){
    axios.get('http://chromely.com/WeatherForecastController/Get')
    .then(response => {
        this.forecasts = this.parseArrayResult(response.data);
    })
    .catch(error => {
        console.log(error);
    });
  }

  parseArrayResult(data: WeatherForecast[]) {
    var dataArray = [];
    for (var i = 0; i < data.length; i++) {
        var tempItem: WeatherForecast = {
          date: data[i].date,
          temperatureC: data[i].temperatureC,
          temperatureF: data[i].temperatureF,
          summary: data[i].summary
        };

        dataArray.push(tempItem);
    }

    return dataArray;
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
