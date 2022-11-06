import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { Project } from '../Models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService{

  baseUrl='https://localhost:7039/api/timely';
  Url='https:localhost:7039/api/timely/CreateSession';
  public first: string = "";
  public prev: string = "";
  public next: string = "";
  public last: string = "";

  constructor(private http: HttpClient) { }  

  getAllProjects():Observable<Project[]>{
    return this.http.get<Project[]>(this.baseUrl+'/AllProjects');
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
  addSession(id:number):Observable<Project>{
    return this.http.get<Project>(this.baseUrl+'/AddSession/'+id);    
  }  
  endSession(id:number): Observable<Project>{
    return this.http.get<Project>(this.baseUrl+'/EndSession/'+id);
  }
}
