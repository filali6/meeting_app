import { HttpParams, HttpResponse } from "@angular/common/http";
import { signal, WritableSignal } from "@angular/core";
import { PaginationResults } from "../_models/Pagination";
import { UserParams } from "../_models/UserParams";

export function getParams(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();
    if (pageNumber && pageSize) {
        params = params.append("pageNumber", pageNumber);
        params = params.append("PageSize", pageSize);
    }

    return params;
}
export function setPaginatedResp<T>(response: HttpResponse<T>, paginatedResultSignal: ReturnType<typeof signal<PaginationResults<T> | null>>) {
    paginatedResultSignal.set({
        items: response.body as T,
        Pagination: JSON.parse(response.headers.get("Pagination")!)
    })
  }
export  function parseJwt (token:string) {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(escape(atob(base64)));
    return JSON.parse(jsonPayload)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
};
