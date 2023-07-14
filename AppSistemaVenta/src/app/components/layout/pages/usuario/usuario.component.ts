import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';

import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';

import { ModalUsuarioComponent } from '../../modales/modal-usuario/modal-usuario.component';
import { Usuario } from 'src/app/interfaces/usuario';
import { UsuarioService } from 'src/app/services/usuario.service';
import { UtilidadService } from 'src/app/reutilizable/utilidad.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit, AfterViewInit {

  columnas: string[] = ['nombreCompleto', 'correo', 'rolDescripcion', 'estado','acciones'];
  dataInicio:Usuario[] = [];
  dataListausuarios = new MatTableDataSource(this.dataInicio);
  @ViewChild(MatPaginator) paginacionTabla!: MatPaginator;

  constructor(private dialog:MatDialog, private usuarioServicio:UsuarioService, private utilidadServicio: UtilidadService){}

  obtenerUsuarios(){
    this.usuarioServicio.lista().subscribe({
      next:(unnombrecualquiera) => {
        if(unnombrecualquiera.status)
          this.dataListausuarios.data = unnombrecualquiera.value;
        else
          this.utilidadServicio.mostrarAlerta("No se encontraron datos","Opps!");
      },
      error:(e) => {}
    })
  }

  ngOnInit(): void {
    this.obtenerUsuarios();
  }

  ngAfterViewInit(): void {
    this.dataListausuarios.paginator = this.paginacionTabla;
  }

  aplicarFiltroTabla(event: Event){
    const filterValue = (event.target as  HTMLInputElement).value;
    this.dataListausuarios.filter = filterValue.trim().toLocaleLowerCase();

  }

  nuevoUsuario(){
    this.dialog.open(ModalUsuarioComponent, {
      disableClose:true
    }).afterClosed().subscribe(result => {
      if(result == "true"){
        this.obtenerUsuarios();
      }
    });
  }

  editarUsuario(usuario:Usuario){
    this.dialog.open(ModalUsuarioComponent, {
      disableClose:true,
      data:usuario
    }).afterClosed().subscribe(result => {
      if(result == "true"){
        this.obtenerUsuarios();
      }
    });
  }

  eliminarUsuario(usuario:Usuario){
    Swal.fire({
      title: 'Desea Eliminar el usuario?',
      text: usuario.nombreCompleto,
      icon:"warning",
      confirmButtonColor:'#3085D6',
      confirmButtonText:"Si, eliminar",
      showCancelButton: true,
      cancelButtonColor: '#d33',
      cancelButtonText: 'No, volver'
    }).then((result) => {
      if(result.isConfirmed){
        this.usuarioServicio.eliminar(usuario.idusuario).subscribe({
          next:(data) =>{
            if(data.status){
              this.utilidadServicio.mostrarAlerta("El usuario fue eliminado","Listo!");
              this.obtenerUsuarios();
            }else{
              this.utilidadServicio.mostrarAlerta("No se pudo eliminar el usuario","Error!");
              this.obtenerUsuarios();
            }
          },
          error:(e)=>{}
        })
      }
    })
  }

}
