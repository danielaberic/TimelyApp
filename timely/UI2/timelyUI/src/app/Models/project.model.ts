export interface Project{
    id:number,
    name:string,
    startDate:string,
    endDate:string,
    total:string
}
export interface Pagination {
    currentPage: number;
    // itemsPerPage: number;
    // totalItems: number;
    hasNext:boolean;
    hasPrevious:boolean;
    totalCount:number;
    totalPages: number;
  }
  
  export class PaginatedResult<T>{
    resut: T;
    pagination: Pagination;
  }