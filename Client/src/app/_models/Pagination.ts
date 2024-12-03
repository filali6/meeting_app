export interface Pagination{
    currentPage:number;
    itemsPerPage:number;
    totalItems : number;
    totalPages : number;
}
export class PaginationResults<T>{
    items?: T;
    Pagination? : Pagination;
}