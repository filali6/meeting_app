import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { authGardGuard } from './_guards/auth-gard.guard';
import { ErrorTesterComponent } from './error-tester/error-tester.component';
import { ServerErrorComponent } from './_errors/server-error/server-error.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';

export const routes: Routes = [
    {path : "",component:HomeComponent},
    {path : "",
        runGuardsAndResolvers : "always",
        canActivate : [authGardGuard],
        children : [
            {path : "lists",component:ListsComponent , canActivate:[authGardGuard]},
            {path : "members",component:MemberListComponent},
            {path : "member/:username",component:MemberDetailComponent},
            {path : "messages",component:MessagesComponent},
            
        ]
    },
    {path : "error",component:ErrorTesterComponent},
    {path : "server-error",component:ServerErrorComponent},
    {path : "**",component:HomeComponent,pathMatch:'full'}
    
];
