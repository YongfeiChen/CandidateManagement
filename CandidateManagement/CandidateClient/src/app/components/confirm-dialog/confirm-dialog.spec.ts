import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmDialog } from './confirm-dialog';

describe('ConfirmDialog', () => {
  let component: ConfirmDialog;
  let fixture: ComponentFixture<ConfirmDialog>;

  /**
   * Setup test module and create component instance before each test.
   */
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConfirmDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  /**
   * Test: ConfirmDialog component should be created successfully.
   */
  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
