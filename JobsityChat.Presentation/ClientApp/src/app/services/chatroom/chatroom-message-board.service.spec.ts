import { TestBed } from '@angular/core/testing';

import { ChatroomMessageBoardService } from './chatroom-message-board.service';

describe('ChatroomMessageBoardService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ChatroomMessageBoardService = TestBed.get(ChatroomMessageBoardService);
    expect(service).toBeTruthy();
  });
});
