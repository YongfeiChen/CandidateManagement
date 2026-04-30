import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>确认删除</h2>
    <mat-dialog-content>
      你确定要删除这位候选人吗？此操作不可撤销。
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onNoClick()">取消</button>
      <!-- [mat-dialog-close] 会在关闭时返回 true -->
      <button mat-raised-button color="warn" [mat-dialog-close]="true">确定删除</button>
    </mat-dialog-actions>
  `
})
export class ConfirmDialogComponent {
  private dialogRef = inject(MatDialogRef<ConfirmDialogComponent>);

  onNoClick(): void {
    this.dialogRef.close(false);
  }
}
