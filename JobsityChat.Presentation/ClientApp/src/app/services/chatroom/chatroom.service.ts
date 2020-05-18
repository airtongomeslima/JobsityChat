import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { ChatRoomModel } from 'src/app/models/ChatRoomModel';
import { BaseResponseModel } from 'src/app/models/baseResponseModel';
import { Observable, BehaviorSubject } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ChatroomService {

  public chatRooms: BehaviorSubject<Array<ChatRoomModel>> = new BehaviorSubject<Array<ChatRoomModel>>(new Array<ChatRoomModel>());
  constructor(private baseService: BaseService) {
  }

  public GetChaRoomList() {
    this.baseService.getList<ChatRoomModel>(null, 'ChatRoom').pipe(take(1)).subscribe(res => {
      this.chatRooms.next(res);
    });


  }

  public CreateChatRoom(chatRoomTitle: string) {
    const newChatRoom = new ChatRoomModel();
    newChatRoom.title = chatRoomTitle;
    newChatRoom.usersCount = 0;
    this.baseService.post<Array<ChatRoomModel>, ChatRoomModel>(newChatRoom, 'ChatRoom').pipe(take(1)).subscribe(res => {
      if (!res.success) {
        alert(res.message);
      } else {
        this.chatRooms.next(res.response);
      }
    });
  }
}
