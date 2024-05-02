import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Component
import { LayoutComponent } from './admin/components/layouts/layout.component';
import { AuthlayoutComponent } from './admin/components/authlayout/authlayout.component';
import { AuthGuard } from './shared/guards/auth.guard';

const routes: Routes = [
  { path: '', component: LayoutComponent, loadChildren: () => import('./admin/components/pages/pages.module').then(m => m.PagesModule), canActivate: [AuthGuard]  },
  { path: 'auth', component: AuthlayoutComponent, loadChildren: () => import('./admin/components/account/account.module').then(m => m.AccountModule) },
  { path: 'pages',component: AuthlayoutComponent, loadChildren: () => import('./admin/components/extraspages/extraspages.module').then(m => m.ExtraspagesModule)},
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'top' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
