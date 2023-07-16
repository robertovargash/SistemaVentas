import { Component,OnInit } from '@angular/core';

import { Chart, registerables } from 'chart.js';
import { DashBoardService } from 'src/app/services/dash-board.service';
Chart.register(...registerables);

@Component({
  selector: 'app-dash-board',
  templateUrl: './dash-board.component.html',
  styleUrls: ['./dash-board.component.css']
})
export class DashBoardComponent implements OnInit{

  totalingresos:string ="0";
  totalVentas:string="0";
  totalProducto:string="0";  

  constructor(private dashboardService:DashBoardService){

  }

  mostrarGrafico(labelGrafico:any[],dataGrafico:any[]){
    const chartBar = new Chart('chartBarra',{
      type: 'bar',
      data: { 
        labels:labelGrafico, 
        datasets:[{
          label:'# de ventas',
          data:dataGrafico,
          backgroundColor:['rgba(54,162,235,0.2)'],
          borderColor: ['rgba(54,162,235,1)'],
          borderWidth: 1
        }]},
      options: {
        maintainAspectRatio: false,
        responsive:true,
        scales: {          
          y: {
            beginAtZero: true
          }
        }
      },
    });
  }

  ngOnInit(): void {
    this.dashboardService.dashBoard().subscribe({
      next:(data) => {
        if(data.status){
          this.totalingresos = data.value.totalIngresos;
          this.totalVentas = data.value.totalVentas;
          this.totalProducto = data.value.totalProductos;
          console.log(data.value);
          const arrayData:any[] = data.value.ventasUltimaSemana;
          
          const labelTemp = arrayData.map((value) => value.fecha);
          const dataTemp = arrayData.map((value) => value.total);
          this.mostrarGrafico(labelTemp,dataTemp);
        }
      }, error:(e)=>{}
    })
  }
}
