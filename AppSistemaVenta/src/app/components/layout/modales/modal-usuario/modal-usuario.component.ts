import { Component, Inject, OnInit, inject,  } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Rol } from 'src/app/interfaces/rol';
import { Usuario } from 'src/app/interfaces/usuario';
import { RolService } from 'src/app/services/rol.service';
import { UsuarioService } from 'src/app/services/usuario.service';
import { UtilidadService } from 'src/app/reutilizable/utilidad.service';

@Component({
  selector: 'app-modal-usuario',
  templateUrl: './modal-usuario.component.html',
  styleUrls: ['./modal-usuario.component.css']
})
export class ModalUsuarioComponent implements OnInit{

  formularioUsuario:FormGroup;
  ocultarPassword:boolean = true;
  tituloAccion:string = "Agregar";
  botonAccion:string = "Guardar";
  listaRoles: Rol[]= [];

  constructor(
    private modalActual:MatDialogRef<ModalUsuarioComponent>,
    @Inject(MAT_DIALOG_DATA) public datosUsuario:Usuario,
    private fb:FormBuilder,
    private rolServicio:RolService,
    private usuarioServicio:UsuarioService,
    private utilidadServicio:UtilidadService
  ){
    this.formularioUsuario = this.fb.group({
      nombreCompleto:['',Validators.required],
      correo:['',Validators.required],
      idRol:['',Validators.required],
      clave:['',Validators.required],
      esActivo:['1',Validators.required],
    });
    if(this.datosUsuario != null){
      this.tituloAccion = "Editar";
      this.botonAccion = "Actualizar";
    }
    this.rolServicio.lista().subscribe({
      next:(unnombrecualquiera) => {
        if(unnombrecualquiera.status)this.listaRoles = unnombrecualquiera.value
      },
      error:(e) => {}
    })
  }

  ocultarPass(){
    this.ocultarPassword = !this.ocultarPassword;
  }

  ngOnInit(): void {
    if(this.datosUsuario != null){
      this.formularioUsuario.patchValue({
        Idusuario: this.datosUsuario.idusuario,
        nombreCompleto:this.datosUsuario.nombreCompleto,
        correo:this.datosUsuario.correo,
        idRol:this.datosUsuario.idRol,
        clave:this.datosUsuario.clave,
        esActivo:this.datosUsuario.esActivo.toString(),
      });
    }
  }

  guardarEditar_Usuario(){
    const usuario:Usuario={
      idusuario: this.datosUsuario == null ? 0 : this.datosUsuario.idusuario,
      nombreCompleto: this.formularioUsuario.value.nombreCompleto,
      correo: this.formularioUsuario.value.correo,
      idRol: this.formularioUsuario.value.idRol,
      rolDesripcion: "",
      clave: this.formularioUsuario.value.clave,
      esActivo: parseInt(this.formularioUsuario.value.esActivo),
    }

    if(this.datosUsuario == null){
      this.usuarioServicio.guardar(usuario).subscribe({
        next:(data) =>{
          if(data.status){
            this.utilidadServicio.mostrarAlerta("El usuario fue registrado","Success");
            this.modalActual.close("true");
          }else{
            this.utilidadServicio.mostrarAlerta("No se pudo registrar el usuario", "Error");
          }
        },
        error:(e)=>{}
      });
    }else{
      this.usuarioServicio.editar(usuario).subscribe({
        next:(data) =>{
         
          if(data.status){
            this.utilidadServicio.mostrarAlerta("El usuario fue actualizado","Success");
            this.modalActual.close("true");
          }else{
            this.utilidadServicio.mostrarAlerta("no se pudo actualizar el usuario", "Error");
          }
        },
        error:(e)=>{}
      });
    }
  }

}
