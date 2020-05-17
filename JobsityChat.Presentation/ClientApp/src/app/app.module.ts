import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { ChatRoomComponent } from './components/chat-room/chat-room.component';
import { MessageComponent } from './components/message/message.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { LoginComponent } from './pages/login/login.component';
import { ChatroomComponent as ChatRoom } from './pages/chatroom/chatroom.component';

@NgModule({
  declarations: [
    AppComponent,
    ChatRoomComponent,
    MessageComponent,
    LoginComponent,
    ChatRoom
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    FontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: LoginComponent, pathMatch: 'full' },
      { path: 'ChatRoom', component: ChatRoom }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
