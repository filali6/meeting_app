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

