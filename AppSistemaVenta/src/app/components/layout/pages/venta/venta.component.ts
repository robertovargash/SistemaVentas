import { Component, Inject, OnInit, inject,  } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from '@angular/material/table';

import { ProductoService } from 'src/app/services/producto.service';
import { VentaService } from 'src/app/services/venta.service';
import { UtilidadService } from 'src/app/reutilizable/utilidad.service';

import { Producto } from 'src/app/interfaces/producto';
import { Venta } from 'src/app/interfaces/venta';
import { DetalleVenta } from 'src/app/interfaces/detalle-venta';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-venta',
  templateUrl: './venta.component.html',
  styleUrls: ['./venta.component.css']
})
export class VentaComponent  implements OnInit{

  listaProductos:Producto[]=[];
  listaProductorFiltro:Producto[]=[];
  listaProductosParaVenta:DetalleVenta[]=[];
  bloquearBotonRegistrar:boolean=false;

  productoSeleccionado!:Producto;
  tipoPagoPorDefecto:string ="Efectivo";
  totalPagar:number=0;

  formularioProductoVenta:FormGroup;
  columnas:string[]=['producto','cantidad','precio','total','accion'];
  datosDetalleventa = new MatTableDataSource(this.listaProductosParaVenta);

  retornanrproductoPorFiltro(busqueda:any):Producto[]{
    const valorBuscado = typeof busqueda == "string" ? busqueda.toLocaleLowerCase() : busqueda.nombre.toLocaleLowerCase();
    return this.listaProductos.filter(item => item.nombre.toLocaleLowerCase().includes(valorBuscado));

  }

  disableRegistrarVenta():boolean{
    return this.listaProductosParaVenta.length < 1 || this.bloquearBotonRegistrar;
  }

  constructor(private fb:FormBuilder,private productoService:ProductoService,private ventaService:VentaService,private utilidadService:UtilidadService){
    this.formularioProductoVenta = this.fb.group({
      producto:['',Validators.required],
      cantidad:['',Validators.required],
      tipoPagoPorDefecto:['',Validators.required],
    });
    this.productoService.lista().subscribe({
      next:(unnombrecualquiera) => {
        if(unnombrecualquiera.status){
          const lista = unnombrecualquiera.value as Producto[];
          this.listaProductos = lista.filter(p=>p.esActivo == 1 && p.stock > 0);
        }
      },
      error:(e) => {}
    });
    this.formularioProductoVenta.get('producto')?.valueChanges.subscribe(value =>{
      this.listaProductorFiltro = this.retornanrproductoPorFiltro(value);
    });

  }

  ngOnInit(): void {
    
  }

  mostrarProducto(producto:Producto):string{
    return producto.nombre;
  }

  productoParaVenta(event: any){
    this.productoSeleccionado = event.option.value;
  }

  agregarProductoParaVenta(){
    const cantidad:number = this.formularioProductoVenta.value.cantidad;
    const precio:number = parseFloat(this.productoSeleccionado.precio);
    const total:number = cantidad*precio;
    this.totalPagar+=total;
    this.listaProductosParaVenta.push({
      idProducto: this.productoSeleccionado.idProducto,
      descripcionProducto: this.productoSeleccionado.nombre,
      cantidad:cantidad,
      precioTexto:String(precio.toFixed(2)),
      totaltexto:String(total.toFixed(2))
    });

    this.datosDetalleventa = new MatTableDataSource(this.listaProductosParaVenta);
    this.formularioProductoVenta.patchValue({
      producto:'',
      cantidad:''
    });
  }

  eliminarProducto(detalle:DetalleVenta){
    this.totalPagar = this.totalPagar - parseFloat(detalle.totaltexto);
    this.listaProductosParaVenta = this.listaProductosParaVenta.filter(p => p.idProducto != detalle.idProducto);
    this.datosDetalleventa = new MatTableDataSource(this.listaProductosParaVenta);
  }


  registrarVenta(){
    if(this.listaProductosParaVenta.length > 0){
      this.bloquearBotonRegistrar = true;
      const request:Venta ={
        tipoPago:this.tipoPagoPorDefecto,
        totaltexto:String(this.totalPagar.toFixed(2)),
        detalleVenta:this.listaProductosParaVenta
      }
      this.ventaService.registrar(request).subscribe({
        next:(data) =>{
          if(data.status){
            this.totalPagar = 0.00;
            this.listaProductosParaVenta= [];
            this.datosDetalleventa = new MatTableDataSource(this.listaProductosParaVenta); 
            Swal.fire({
              icon:'success',
              title:'Venta registrada!',
              text:`Numero de venta ${data.value.numeroDocumento}`
            })
          }else{
            this.utilidadService.mostrarAlerta("No se pudo registrar la venta","Opps!");
          }
        },
        complete:()=>{
          this.bloquearBotonRegistrar = false;
        },
        error:(e)=>{
          console.log(e);
        }
      })
    }
  }

}
