import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CandidateService {
  private http = inject(HttpClient);

  // 1. Base URL: Configure for environment (production or local)
  private baseUrl = 'https://candidatemanagement-production.up.railway.app/api'; // Replace with your cloud address
  //private baseUrl = 'https://localhost:7227/api'; // Replace with your local address
  // 2. API resource endpoints
  private candidatesUrl = `${this.baseUrl}/candidates`;
  private jobTitlesUrl = `${this.baseUrl}/jobtitles`;
  private skillsUrl = `${this.baseUrl}/skills`;

  /**
   * Retrieves a paginated list of candidates using backend filter endpoint.
   * @param search - Legacy search keyword (ignored by current /filter endpoint)
   * @param pageNumber - Page number for pagination (1-indexed)
   * @param pageSize - Number of items per page (default: 5)
   * @param jobTitleId - Job title ID filter (optional)
   * @param skillIds - Skill IDs filter array (optional)
   * @returns Observable containing filtered candidate list and total count
   */
  getList(
    search: string = '',
    pageNumber: number = 1,
    pageSize: number = 5,
    jobTitleId: number | null = null,
    skillIds: number[] = []
  ): Observable<any> {
    return this.filterCandidates(jobTitleId, skillIds.join(','), pageNumber, pageSize);
  }

  /**
   * Filters candidates by job title and skills with pagination support.
   * @param jobTitleId - Job title ID (optional, filters candidates by specific job title)
   * @param skillIds - Comma-separated skill IDs string (optional, filters candidates with matching skills)
   * @param pageNumber - Page number for pagination (1-indexed, default: 1)
   * @param pageSize - Number of items per page (default: 5)
   * @returns Observable containing filtered candidate list and total count
   */
  filterCandidates(
    jobTitleId: number | null = null,
    skillIds: string = '',
    pageNumber: number = 1,
    pageSize: number = 5
  ): Observable<any> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (jobTitleId !== null) {
      params = params.set('jobTitleId', jobTitleId.toString());
    }

    if (skillIds && skillIds.trim().length > 0) {
      params = params.set('skillIds', skillIds);
    }

    return this.http.get<any>(`${this.candidatesUrl}/filter`, { params });
  }

  /**
   * Creates a new candidate record.
   * @param candidate - Candidate object with required fields
   * @returns Observable of the created candidate data
   */
  add(candidate: any): Observable<any> {
    return this.http.post(this.candidatesUrl, candidate);
  }

  /**
   * Updates an existing candidate record.
   * @param id - Candidate ID
   * @param candidate - Candidate object with updated fields
   * @returns Observable of the update operation result
   */
  update(id: number, candidate: any): Observable<any> {
    return this.http.put(`${this.candidatesUrl}/${id}`, candidate);
  }

  /**
   * Deletes a candidate record by ID.
   * @param id - Candidate ID to delete
   * @returns Observable of the delete operation
   */
  deleteCandidate(id: number): Observable<void> {
    return this.http.delete<void>(`${this.candidatesUrl}/${id}`);
  }

  /**
   * Retrieves all available job titles.
   * @returns Observable containing array of job title objects
   */
  getJobTitles(): Observable<any[]> {
    // Previously used localhost, now using cloud endpoint
    return this.http.get<any[]>(this.jobTitlesUrl);
  }

  /**
   * Retrieves all available skills.
   * @returns Observable containing array of skill objects
   */
  getSkills(): Observable<any[]> {
    // Previously used localhost, now using cloud endpoint
    return this.http.get<any[]>(this.skillsUrl);
  }
}
