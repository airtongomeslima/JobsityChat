import { Component, OnInit, OnDestroy } from '@angular/core';

import { faSearch, faPaperPlane, faPlus } from '@fortawesome/free-solid-svg-icons';
import { ChatRoomModel } from 'src/app/models/ChatRoomModel';
import { ChatroomService } from 'src/app/services/chatroom/chatroom.service';
import { Subscription } from 'rxjs';
import { ChatroomMessageBoardService } from 'src/app/services/chatroom/chatroom-message-board.service';
import { MessageModel } from 'src/app/models/MessageModel';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit, OnDestroy {

  chatRoomlist: Array<ChatRoomModel> = new Array<ChatRoomModel>();
  chatRoom: ChatRoomModel = new ChatRoomModel();
  chatRoomMessages: Array<MessageModel> = new Array<MessageModel>();
  newChatRoomTitle: string;
  newMessage: string;

  title = 'chat';
  faSearch = faSearch;
  faPaperPlane = faPaperPlane;
  faPlus = faPlus;

  subscriptions: Subscription = new Subscription();

  constructor(private chatRoomService: ChatroomService, private chatRoomMessageService: ChatroomMessageBoardService) {
    this.subscriptions.add(this.chatRoomService.chatRooms.subscribe(c => {
      this.chatRoomlist = c;
    }));

    this.subscriptions.add(this.chatRoomMessageService.messages.subscribe(c => {
      this.chatRoomMessages = c;
    }));

    this.chatRoomService.GetChaRoomList();
  }

  ngOnInit(): void {
  }

  createChatRoom() {
    this.chatRoomService.CreateChatRoom(this.newChatRoomTitle);
    this.newChatRoomTitle = '';
  }

  selectChatRoom(chatRoomId: number) {
    this.chatRoomlist.map(c => {
      if (c.chatRoomId === chatRoomId) {
        this.chatRoom = c;
      }
    });
    this.chatRoomMessageService.GetChaRoomMessages(chatRoomId);
  }

  sendMessage() {
    this.chatRoomMessageService.SendMessage(this.chatRoom.chatRoomId, this.newMessage);
    this.newMessage = '';
  }

  ngOnDestroy(): void {
    if (this.subscriptions) {
      this.subscriptions.unsubscribe();
    }
  }
}
