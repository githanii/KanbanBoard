import { TestBed } from '@angular/core/testing';

import { BoardDetailService } from './board-detail.service';

describe('BoardDetailService', () => {
  let service: BoardDetailService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BoardDetailService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
