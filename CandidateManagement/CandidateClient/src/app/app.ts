import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms'; // 表单必备
import { CandidateService } from './services/candidate.service';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { Subject, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog';
import { MatIconModule } from '@angular/material/icon'
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
  MatPaginatorModule,
  MatIconModule,
  MatDialogModule,
  CommonModule, 
  ReactiveFormsModule, 
  CommonModule, 
  ReactiveFormsModule, 
  MatTableModule, 
  MatFormFieldModule, 
  MatInputModule, 
  MatButtonModule,
  MatSelectModule, 
  MatCardModule,
  MatChipsModule], // 导入模块
  templateUrl: './app.html'
})
export class App implements OnInit {
  private service = inject(CandidateService);
  private fb = inject(FormBuilder);
  private searchTerms = new Subject<string>()
  private dialog = inject(MatDialog)
  candidates = signal<any[]>([]); 
  jobTitles = signal<any[]>([]); 
  allSkills = signal<any[]>([]);
  totalCount = signal(0);
  pageNumber = 1;
  pageSize = 5;

  // 定义表单结构
  candidateForm = this.fb.group({
    name: ['', Validators.required],
    jobTitleId: [null, Validators.required],
    skillIds: [[]], // 技能可以是多个，所以用数组
    status: ['Applied']
  });

  ngOnInit() {
      this.loadInitialData();
      this.searchTerms.pipe(
      debounceTime(400),        // 等待 400 毫秒，用户停手后再发请求（防抖）
      distinctUntilChanged(),   // 只有内容真的变了才发请求
      switchMap((term: string) => this.service.getList(term)) // 如果新请求来了，取消旧请求
    ).subscribe(data => {
      this.candidates = data;
    });
    this.service.getJobTitles().subscribe(data => this.jobTitles.set(data));
    this.service.getSkills().subscribe(data => this.allSkills.set(data));
    this.refresh();
  }
   loadInitialData() {
    // 3. 调用 Service 获取职位列表并填充到信号中
    this.service.getJobTitles().subscribe({
      next: (data) => this.jobTitles.set(data),
      error: (err) => console.error('获取职位失败:', err)
    });}
  search(term: string): void {
    this.searchTerms.next(term);
  }
  
 refresh(search: string = '') {
  this.service.getList(search, this.pageNumber, this.pageSize).subscribe(res => {
    this.candidates.set(res.items); // 注意：这里要取 res.items，因为后端返回结构变了
    this.totalCount.set(res.totalCount);
  });
}
onPageChange(event: PageEvent) {
  this.pageNumber = event.pageIndex + 1; // Index 从 0 开始，所以加 1
  this.pageSize = event.pageSize;
  this.refresh();
}

  onSubmit() {
    if (this.candidateForm.valid) {
      this.service.add(this.candidateForm.value).subscribe(() => {
        this.refresh(); // 刷新列表
        this.candidateForm.reset({ status: 'Applied' }); // 重置表单
      });
    }
  }
  
  updateStatus(candidate: any, newStatus: string) {
  const updatedCandidate = { ...candidate, status: newStatus };
  this.service.update(candidate.id, updatedCandidate).subscribe(() => {
    console.log('状态更新成功');
    this.refresh(); // 刷新列表确保数据同步
  });
  }
    deleteCandidate(id: number): void {
    // 1. 打开弹窗
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px'
    });

    // 2. 监听弹窗关闭后的结果
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        // 用户点击了“确定”
        this.service.deleteCandidate(id).subscribe(() => {
          this.refresh();
        });
      }
      });
    }
}

