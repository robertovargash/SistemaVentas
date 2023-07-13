import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LayoutComponent } from './components/layout/layout.component';
import { LayoutRoutingModule } from './components/layout/layout-routing.module';

const routes: Routes = [
  {path:'', component:LoginComponent, pathMatch:"full"},
  {path:'login', component:LoginComponent, pathMatch:"full"},
  {path:'pages', loadChildren: () => import("./components/layout/layout.module").then(m => m.LayoutModule)},//obtiene todas las páginas
  {path:'**', redirectTo:'login',pathMatch:"full"},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
