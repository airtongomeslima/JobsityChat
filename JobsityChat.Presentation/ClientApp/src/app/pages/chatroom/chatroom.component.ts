import { Component, OnInit } from '@angular/core';

import { faSearch, faPaperPlane, faPlus } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-chatroom',
  templateUrl: './chatroom.component.html',
  styleUrls: ['./chatroom.component.css']
})
export class ChatroomComponent implements OnInit {

  title = 'chat';
  faSearch = faSearch;
  faPaperPlane = faPaperPlane;
  faPlus = faPlus;

  constructor() { }

  ngOnInit(): void {
  }

}
