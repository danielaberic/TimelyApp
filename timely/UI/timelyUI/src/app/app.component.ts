import { Component, OnInit } from '@angular/core';
import { Project } from './Models/project.model';
import { ModalServiceService } from './service/modal-service.service';
import { ProjectService } from './service/project.service';
import { RunningProjectService } from './service/running-project.service';
import {faWindowClose} from '@fortawesome/free-solid-svg-icons';
import { faClose } from '@fortawesome/free-solid-svg-icons';
import {faTrash} from '@fortawesome/free-solid-svg-icons';
import * as XLSX from 'xlsx';

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

  projects:Project[]=[];
  project:Project={
    id: 0,
    name:'',
    startDate:Date.parse('yyyy/MM/DD HH:MM'),
    endDate:'',
    total: ''
  };

  constructor(public modalService:ModalServiceService, 
    public runningProject:RunningProjectService, 
    public projectService:ProjectService){

  }
  ngOnInit(): void {
    this.getAllProjects();
  }
  getAllProjects(){
    this.projectService.getAllProjects()
    .subscribe(
      response=>{
        this.projects=response;
      }
    );
  }  

  addProject(){    
    this.projectService.addProject()
    .subscribe(
      response=>{
        this.getAllProjects();
        this.project=response;
      }
    );    
}
  deleteProject(id:number){
    this.projectService.deleteProject(id)
    .subscribe(
      response=>{
        this.getAllProjects();
      }
    );
  }
  updateProject(project:Project){
    this.projectService.updateProject(project)
    .subscribe(
      response=>{
        console.log(response);
        this.getAllProjects();
      }
    );
  }

  populateForm(project:Project){
    this.project=project;
    this.modalService.showModal=true;
    this.runningProject.start();
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
    this.getAllProjects();
  }
  tableSizeChange(event:any):void{
    this.tableSize=event.target.value;
    this.page=1;
    this.getAllProjects();
  }
}
