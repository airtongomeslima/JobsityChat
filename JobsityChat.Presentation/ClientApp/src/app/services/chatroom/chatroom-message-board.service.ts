import { Injectable } from '@angular/core';
import { BehaviorSubject, interval, Observable, Subscription } from 'rxjs';
import { BaseService, QueryOptions } from '../base.service';
import { take } from 'rxjs/operators';
import { MessageModel } from 'src/app/models/MessageModel';

@Injectable({
  providedIn: 'root'
})
export class ChatroomMessageBoardService {

  public messages: BehaviorSubject<Array<MessageModel>> =
    new BehaviorSubject<Array<MessageModel>>(new Array<MessageModel>());
  sub: Subscription = new Subscription();
  constructor(private baseService: BaseService) { }

  public GetChaRoomMessages(chatRoomId: number) {

    this.sub.unsubscribe();

    this.sub = interval(3000).subscribe(x => {
      this.baseService.getList<MessageModel>(null, 'ChatRoomMessageBoard/' + chatRoomId).pipe(take(1)).subscribe(res => {
        this.messages.next(res);
      });
      console.log('interval', chatRoomId);
    });


  }

  public SendMessage(chatRoomId: number, message: string) {

    const newMessage = new MessageModel();
    newMessage.chatRoomId = chatRoomId;
    newMessage.text = message;
    console.log('newMessage', newMessage);
    this.baseService.post<Array<MessageModel>, MessageModel>(newMessage, 'ChatRoomMessageBoard').pipe(take(1)).subscribe(res => {
      console.log('response', res);
      if (!res.success) {
        alert(res.message);
      } else {
        this.messages.next(res.response);
      }
    });
  }

}
