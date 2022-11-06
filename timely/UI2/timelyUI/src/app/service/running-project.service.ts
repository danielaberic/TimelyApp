import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RunningProjectService {

  constructor() { }
  running=false;

  start():void{
    if(!this.running){
      this.running=true;
    }else
    this.stop();
  }
  stop():void{
    this.running=false;
  }
}
