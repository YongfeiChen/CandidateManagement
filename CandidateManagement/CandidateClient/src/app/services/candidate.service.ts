import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CandidateService {
  private http = inject(HttpClient);

  // 1. 基础域名：建议定义一个 base，方便统一修改
  private baseUrl = 'https://candidatemanagement-production.up.railway.app/api'; // 替换为你的云端地址

  // 2. 各个资源的具体地址
  private candidatesUrl = `${this.baseUrl}/candidates`;
  private jobTitlesUrl = `${this.baseUrl}/jobtitles`;
  private skillsUrl = `${this.baseUrl}/skills`;

  getList(search: string = '', pageNumber: number = 1, pageSize: number = 5): Observable<any> {
    let params = new HttpParams()
      .set('search', search)
      .set('pageNumber', pageNumber.toString()) // 确保转为字符串
      .set('pageSize', pageSize.toString());

    return this.http.get<any>(this.candidatesUrl, { params });
  }

  add(candidate: any): Observable<any> {
    return this.http.post(this.candidatesUrl, candidate);
  }

  update(id: number, candidate: any): Observable<any> {
    return this.http.put(`${this.candidatesUrl}/${id}`, candidate);
  }

  deleteCandidate(id: number): Observable<void> {
    return this.http.delete<void>(`${this.candidatesUrl}/${id}`);
  }

  getJobTitles(): Observable<any[]> {
    // 之前这里写的是 localhost，现在必须改成云端地址
    return this.http.get<any[]>(this.jobTitlesUrl);
  }

  getSkills(): Observable<any[]> {
    // 之前这里写的是 localhost，现在必须改成云端地址
    return this.http.get<any[]>(this.skillsUrl);
  }
}
