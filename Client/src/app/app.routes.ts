import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { authGardGuard } from './_guards/auth-gard.guard';

export const routes: Routes = [
    {path : "",component:HomeComponent},
    {path : "",
        runGuardsAndResolvers : "always",
        canActivate : [authGardGuard],
        children : [
            {path : "lists",component:ListsComponent , canActivate:[authGardGuard]},
            {path : "members",component:MemberListComponent},
            {path : "members/:id",component:HomeComponent},
            {path : "messages",component:MessagesComponent},
        ]
    },
    
    {path : "**",component:HomeComponent,pathMatch:'full'}
    
];
