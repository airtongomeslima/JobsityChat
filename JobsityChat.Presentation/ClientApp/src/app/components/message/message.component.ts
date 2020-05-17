import { Component, OnInit, Input } from '@angular/core';
import { faComments } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  @Input() userName: string;
  @Input() date: Date;
  @Input() text: string;
  @Input() isIn: boolean;
  faComments = faComments;

  constructor() { }

  ngOnInit(): void {
  }

}
