import { Component, OnInit, Input } from '@angular/core';
import { faComments } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-chat-room',
  templateUrl: './chat-room.component.html',
  styleUrls: ['./chat-room.component.css']
})
export class ChatRoomComponent implements OnInit {
  @Input() name: string;
  @Input() id: number;
  @Input() users: number;
  faComments = faComments;

  constructor() { }

  ngOnInit(): void {
  }

}
