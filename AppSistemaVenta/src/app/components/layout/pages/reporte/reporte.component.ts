import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { MAT_DATE_FORMATS } from "@angular/material/core";
import * as moment from 'moment';
import * as XLSX from "xlsx";

import { Reporte } from 'src/app/interfaces/reporte';
import { VentaService } from 'src/app/services/venta.service';
import { UtilidadService } from 'src/app/reutilizable/utilidad.service';

export const MY_DATA_FORMATS={
  parse:{
    dateInput:'DD/MM/YYYY'
  },
  display:{
    dateInput:'DD/MM/YYYY',
    monthYearLabel:'MMMM YYYY'
  }
}

@Component({
  selector: 'app-reporte',
  templateUrl: './reporte.component.html',
  styleUrls: ['./reporte.component.css'],
  providers: [{ provide: MAT_DATE_FORMATS, useValue: MY_DATA_FORMATS }],
})
export class ReporteComponent implements OnInit, AfterViewInit{

  formularioFiltro: FormGroup;
  listaVentasReporte:Reporte[]=[];
  columnasTabla:string[]=['fechaRegistro','numeroVenta','tipoPago','total','producto','cantidad','precio','totalProducto'];
  dataVentaReporte = new MatTableDataSource(this.listaVentasReporte);
  @ViewChild(MatPaginator) paginacionTabla!: MatPaginator;

  constructor(private fb:FormBuilder, private ventaService:VentaService, private utilidadService:UtilidadService ){
    this.formularioFiltro = this.fb.group({
      fechaInicio:['',Validators.required],
      fechaFin:['',Validators.required]
    });
  }

  ngOnInit(): void {
    
  }

  ngAfterViewInit(): void {
    this.dataVentaReporte.paginator = this.paginacionTabla;
  }

  buscarVentas(){
    const fechaInicio:string = moment(this.formularioFiltro.value.fechaInicio).format('DD/MM/YYYY');
    const fechaFin:string = moment(this.formularioFiltro.value.fechaFin).format('DD/MM/YYYY');

    if(fechaInicio === "Invalid date" || fechaFin === "Invalid date"){
      this.utilidadService.mostrarAlerta("Debe ingresar ambas fechas correctamente","Oopps!");
      return;
    }

    this.ventaService.reporte(fechaInicio, fechaFin).subscribe({
      next:(data)=>{
        if(data.status){
          this.listaVentasReporte = data.value;
          this.dataVentaReporte = data.value;
        }          
        else{
          this.listaVentasReporte = [];
          this.dataVentaReporte.data = [];
          this.utilidadService.mostrarAlerta("No se encontraron datos","Oopss!");
        }
      },
      error:(e)=>{}
    })
  }

  exportarExcel(){
    const wb = XLSX.utils.book_new();
    const ws = XLSX.utils.json_to_sheet(this.listaVentasReporte);

    XLSX.utils.book_append_sheet(wb,ws,"Reporte");
    XLSX.writeFile(wb,"Reporte Ventas.xlsx");    
  }
}
