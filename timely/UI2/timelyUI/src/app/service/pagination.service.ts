import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from "@angular/common/http";
import { PaginatedResult, Project } from '../Models/project.model';
import { map, catchError } from 'rxjs/operators';
import { Observable } from "rxjs";
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaginationService {
  projects:Project[];
  baseUrl='https://localhost:7039/api/timely';

  constructor(private httpClient: HttpClient) { }

  getAllProjects(pageNumber?:number, pageSize?:number): Observable<any> {

    const paginatedResults: PaginatedResult<Project[]|null> = new PaginatedResult<Project[]>();

    let params = new HttpParams();

    if (pageNumber != null && pageSize != null) {
      params = params.append('Page', pageNumber);
      params = params.append('ItemsPerPage', pageSize);
    }

    return this.httpClient.get<Project[]>(
      this.baseUrl,
      {params: params,observe:'response'}).
      pipe(map(res => {
        paginatedResults.resut = res.body;
        if (res.headers.get('X-Pagination') != null) {
          paginatedResults.pagination = JSON.parse(res.headers.get('X-Pagination')|| '{}')
          console.log(paginatedResults.pagination);
        }
        return paginatedResults;
      })
      );
  }
}
