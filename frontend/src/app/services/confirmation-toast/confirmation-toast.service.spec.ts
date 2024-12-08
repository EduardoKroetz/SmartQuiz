import { TestBed } from '@angular/core/testing';

import { ConfirmationToastService } from './confirmation-toast.service';

describe('ConfirmationToastService', () => {
  let service: ConfirmationToastService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfirmationToastService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
