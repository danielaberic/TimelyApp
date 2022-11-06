import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../Models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService{

  baseUrl='https://localhost:7014/api/timely';
  Url='https://localhost:7014/api/timely/StartSession';

  constructor(private http: HttpClient) { }
  

  //get all projects
  getAllProjects():Observable<Project[]>{
    return this.http.get<Project[]>(this.baseUrl);
  }
  //add project
  addProject():Observable<Project>{
    return this.http.get<Project>(this.Url);    
  }
  deleteProject(id:number): Observable<Project>{
    return this.http.delete<Project>(this.baseUrl+'/'+id);
  }

  updateProject(project:Project): Observable<Project>{
    return this.http.put<Project>(this.baseUrl+'/'+project.id,project);
  }  
}
