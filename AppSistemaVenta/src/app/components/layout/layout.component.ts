import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Menu } from 'src/app/interfaces/menu';
import { MenuService } from 'src/app/services/menu.service';
import { UtilidadService } from 'src/app/reutilizable/utilidad.service';


@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit{

  listaMenus:Menu[]=[];
  correoUsuario:string='';
  rolUsuario:string='';

  constructor(private router:Router, private menuService:MenuService, private utilidadService:UtilidadService){

  }

  ngOnInit(): void {
    const usuario = this.utilidadService.obtenerSesionusuario();
    if(usuario != null){
      this.correoUsuario = usuario.correo;
      this.rolUsuario = usuario.rolDescripcion;
      this.menuService.lista(usuario.idusuario).subscribe({
        next:(data)=>{
          if(data.status) this.listaMenus = data.value;
        },
        error:(e)=>{}
      })
    }
  }

  cerrarSesion(){
    this.utilidadService.eliminarSesionUsuario();
    this.router.navigate(['login']);
  }
}
