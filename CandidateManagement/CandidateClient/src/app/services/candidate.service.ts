import { inject, Injectable } from '@angular/core';
import { HttpClient,HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CandidateService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7227/api/candidates'; // 记得换成你后端的端口

  getList(search: string = '', pageNumber: number = 1, pageSize: number = 5): Observable<any> {
    let params = new HttpParams()
      .set('search', search)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<any>(this.apiUrl, { params });
  }


  add(candidate: any): Observable<any> {
    return this.http.post(this.apiUrl, candidate);
  }
  update(id: number, candidate: any): Observable<any> {
  return this.http.put(`${this.apiUrl}/${id}`, candidate);
}
deleteCandidate(id: number): Observable<void> {
  return this.http.delete<void>(`${this.apiUrl}/${id}`);
}

getJobTitles(): Observable<any[]> {
  return this.http.get<any[]>(`https://localhost:7227/api/jobtitles`);
}
getSkills(): Observable<any[]> {
  return this.http.get<any[]>(`https://localhost:7227/api/skills`);
}
}
