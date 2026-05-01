import { TestBed } from '@angular/core/testing';

import { CandidateService } from './candidate.service';

describe('CandidateService', () => {
  let service: CandidateService;

  /**
   * Setup test module and inject CandidateService before each test.
   */
  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CandidateService);
  });

  /**
   * Test: CandidateService should be created and injected successfully.
   */
  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
