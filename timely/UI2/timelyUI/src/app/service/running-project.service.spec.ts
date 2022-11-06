import { TestBed } from '@angular/core/testing';

import { RunningProjectService } from './running-project.service';

describe('RunningProjectService', () => {
  let service: RunningProjectService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RunningProjectService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
