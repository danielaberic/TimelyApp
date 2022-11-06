import { Component, OnInit } from '@angular/core';
import { PaginatedResult, Project } from './Models/project.model';
import { ModalServiceService } from './service/modal-service.service';
import { ProjectService } from './service/project.service';
import { RunningProjectService } from './service/running-project.service';
import {faWindowClose} from '@fortawesome/free-solid-svg-icons';
import { faClose } from '@fortawesome/free-solid-svg-icons';
import {faTrash} from '@fortawesome/free-solid-svg-icons';
import * as XLSX from 'xlsx';
import { PaginationService } from './service/pagination.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'timelyUI';
  faClose=faClose;
  faTrash=faTrash;
  faWindowClose=faWindowClose;
  fileName="table.xlsx";
  page:number=1;
  count:number=0;
  tableSize:number=10;
  tableSizes:any=[5,10,15,20];

  config: any;

  projects:Project[]=[];
  project:Project={
    id: 0,
    name:'',
    startDate:'',
    endDate:'',
    total: ''
  };

  constructor(public modalService:ModalServiceService, 
    public runningProject:RunningProjectService, 
    public projectService:ProjectService,
    public paginationService:PaginationService
    ){

  }
  ngOnInit(): void {
    this.getPaginatedProjects(this.page,this.tableSize);
    this.config = {
      itemsPerPage: this.tableSize,
      currentPage: this.page,
      totalItems: 5
    }
  }
  // getAllProjects(){
  //   this.dataService.getAllProjects()
  //   .subscribe(
  //     response=>{
  //       this.projects=response;
  //       console.log(response);
  //     }
  //   );
  // }  
  
  getPaginatedProjects(page:number,tableSize:number){
    this.paginationService.getAllProjects(page,tableSize)
      .subscribe((response: PaginatedResult<Project[]>) => {
        console.log(response.pagination);
        this.projects = response.resut;
        console.log(this.projects);
        this.config = {          
          currentPage: response.pagination.currentPage,
          totalItems: response.pagination.totalCount
        };
      }
      );
  }  

  addProject(){    
    this.projectService.addProject()
    .subscribe(
      response=>{
        this.getPaginatedProjects(this.config.currentPage,this.tableSize);
        this.project=response;
      }
    ); 
    this.runningProject.start();   
}
  deleteProject(id:number){
    this.projectService.deleteProject(id)
    .subscribe(
      response=>{
        this.getPaginatedProjects(this.config.currentPage,this.tableSize);
      }
    );
    this.runningProject.stop();
    this.modalService.showModal=false;
  }
  addSession(id:number){
    this.projectService.addSession(id)
    .subscribe(
      response=>{
        this.getPaginatedProjects(this.config.currentPage,this.tableSize);
      }
    );
    this.modalService.showModal=false;
    this.runningProject.start();
  }
  endSession(id:number){
    this.projectService.endSession(id)
    .subscribe(
      response=>{
        this.getPaginatedProjects(this.config.currentPage,this.tableSize);
      }
    );
    this.modalService.showModal=true
    this.runningProject.stop();
  }
  updateProject(project:Project){
    this.projectService.updateProject(project)
    .subscribe(
      response=>{
        console.log(response);
        this.getPaginatedProjects(this.config.currentPage,this.tableSize);
      }
    );
    this.runningProject.stop();
    this.modalService.showModal=false;
  }

  populateForm(project:Project){
    this.project=project;
    this.modalService.showModal=true;
  }
  exportToExcel():void{
    var element=document.getElementById('table');
    var ws:XLSX.WorkSheet=XLSX.utils.table_to_sheet(element);
    var wb:XLSX.WorkBook=XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(wb,ws,'Sheet1');
    XLSX.writeFile(wb,this.fileName);
  }

  pagination(event:any){
    this.page=event;
    //this.getAllProjects();
    this.config.totalItems;
    this.getPaginatedProjects(event,this.tableSize);
  }
  tableSizeChange(event:any):void{
    this.tableSize=event.target.value;
    this.page=1;
    this.pagination(this.page);
  }  
}
