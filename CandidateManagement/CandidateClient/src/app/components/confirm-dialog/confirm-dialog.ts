import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Confirm Delete</h2>
    <mat-dialog-content>
      Are you sure you want to delete this candidate? This action cannot be undone.
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onNoClick()">Cancel</button>
      <!-- [mat-dialog-close] returns true when closing -->
      <button mat-raised-button color="warn" [mat-dialog-close]="true">Confirm Delete</button>
    </mat-dialog-actions>
  `
})
export class ConfirmDialogComponent {
  private dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);

  /**
   * Closes the dialog without confirming deletion.
   */
  onNoClick(): void {
    this.dialogRef.close(false);
  }
}
