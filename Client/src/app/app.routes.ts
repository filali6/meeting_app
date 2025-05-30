import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { authGardGuard } from './_guards/auth-gard.guard';
import { ErrorTesterComponent } from './error-tester/error-tester.component';
import { ServerErrorComponent } from './_errors/server-error/server-error.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { editGuardGuard } from './_guards/edit-guard.guard';
import { memberDetailResolver } from './_resolvers/member-detail.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './_guards/admin.guard';

export const routes: Routes = [
    {path : "",component:HomeComponent},
    {path : "",
        runGuardsAndResolvers : "always",
        canActivate : [authGardGuard],
        children : [
            {path : "lists",component:ListsComponent},
            {path : "members",component:MemberListComponent},
            {path : "member/:username",component:MemberDetailComponent ,resolve:[memberDetailResolver]},
            {path : "edit",component:MemberEditComponent,canDeactivate :[editGuardGuard]},
            {path : "messages",component:MessagesComponent},
            {path : "admin",component:AdminPanelComponent, canActivate:[adminGuard]}
        ]
    },
    {path : "error",component:ErrorTesterComponent},
    {path : "server-error",component:ServerErrorComponent},
    {path : "**",component:HomeComponent,pathMatch:'full'}
    
];
