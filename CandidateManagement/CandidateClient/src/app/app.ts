import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms'; // Required for form handling
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
import { MatToolbarModule } from '@angular/material/toolbar';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
  MatToolbarModule,
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
  MatChipsModule], // Import required modules
  templateUrl: './app.html',
  styleUrl: './app.scss' 
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

  // Define form structure with validation rules for candidate creation
  candidateForm = this.fb.group({
    name: ['', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50),
      // Regex pattern matching backend validation
      Validators.pattern(/^[\u4e00-\u9fa5a-zA-Z\s·]+$/)
    ]],
    jobTitleId: [null, Validators.required],
    skillIds: [[]], // Skills array supports multiple selections
    status: ['Applied']
  });

  // Filter controls for candidate search
  filterForm = this.fb.group({
    search: [''],
    jobTitleId: [null],
    skillIds: [[] as number[]]
  });

  /**
   * Angular lifecycle hook: Initializes component data and subscriptions.
   */
  ngOnInit() {
    this.loadInitialData();

    this.searchTerms.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      switchMap((term: string) => {
        this.pageNumber = 1; // Reset page number when searching
        this.filterForm.patchValue({ search: term }, { emitEvent: false });
        return this.service.getList(
          term,
          this.pageNumber,
          this.pageSize,
          this.filterForm.value.jobTitleId,
          this.filterForm.value.skillIds ?? []
        );
      })
    ).subscribe({
      next: (res) => {
        // Important: Table data source must be an array, extract items from response
        this.candidates.set(res.items);
        // Paginator requires total count
        this.totalCount.set(res.totalCount);
      },
      error: (err) => console.error('Search error', err)
    });

    this.refresh();
  }

  /**
   * Loads initial lookup data including job titles and skills.
   */
  loadInitialData() {
    this.service.getJobTitles().subscribe({
      next: (data) => this.jobTitles.set(data),
      error: (err) => console.error('Failed to load job titles:', err)
    });

    this.service.getSkills().subscribe({
      next: (data) => this.allSkills.set(data),
      error: (err) => console.error('Failed to load skills:', err)
    });
  }

  /**
   * Triggers search with debouncing to filter candidates by keyword.
   * @param term - Search keyword (name or job title)
   */
  search(term: string): void {
    this.searchTerms.next(term);
  }

  /**
   * Applies the selected filters and refreshes the candidate list.
   */
  applyFilters(): void {
    this.pageNumber = 1;
    this.refresh();
  }

  /**
   * Clears all search filters and refreshes the candidate list.
   */
  clearFilters(): void {
    this.filterForm.reset();
    this.filterForm.get('search')?.setValue('');
    this.filterForm.get('jobTitleId')?.setValue(null);
    this.filterForm.get('skillIds')?.setValue([]);
    this.pageNumber = 1;
    this.refresh();
  }

  /**
   * Refreshes the candidate list based on active filter values.
   */
  refresh() {
    const filters = this.filterForm.value;
    this.service.getList(
      filters.search ?? '',
      this.pageNumber,
      this.pageSize,
      filters.jobTitleId,
      filters.skillIds ?? []
    ).subscribe({
      next: (res) => {
        this.candidates.set(res.items); // Note: Extract items array from response
        this.totalCount.set(res.totalCount);
      },
      error: (err) => console.error('Failed to refresh candidates:', err)
    });
  }
/**
   * Handles pagination change events.
   * @param event - PageEvent from mat-paginator
   */
onPageChange(event: PageEvent) {
  this.pageNumber = event.pageIndex + 1; // Index starts at 0, so add 1
  this.pageSize = event.pageSize;
  this.refresh();
}

  /**
   * Submits the candidate form to create a new candidate.
   * Refreshes the list and resets the form on success.
   */
  onSubmit() {
    if (this.candidateForm.valid) {
      this.service.add(this.candidateForm.value).subscribe(() => {
        this.refresh(); // Refresh candidate list
        this.candidateForm.reset({ status: 'Applied' }); // Reset form
      });
    }
  }
  
  /**
   * Updates a candidate's status and refreshes the list.
   * @param candidate - The candidate object to update
   * @param newStatus - The new status value
   */
  updateStatus(candidate: any, newStatus: string) {
  // Important: Build payload with only required backend UpdateDto fields
  const updatePayload = {
    id: candidate.id,
    name: candidate.name,
    status: newStatus,
    // Note: Ensure ReadDto includes jobTitleId numeric field
    jobTitleId: candidate.jobTitleId 
  };

  console.log('Sending update data:', updatePayload);

  this.service.update(candidate.id, updatePayload).subscribe({
    next: () => {
      console.log('Status updated successfully');
      this.refresh(); 
    },
    error: (err) => {
      console.error('Update failed:', err);
      // 400: Field name mismatch; 500: Backend DTO type mismatch
    }
  });
  }
    /**
     * Opens confirmation dialog before deleting a candidate.
     * @param id - Candidate ID to delete
     */
    deleteCandidate(id: number): void {
    // 1. Open confirmation dialog
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '350px'
    });

    // 2. Listen for dialog result
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        // User confirmed deletion
        this.service.deleteCandidate(id).subscribe(() => {
          this.refresh();
        });
      }
      });
    }
}

