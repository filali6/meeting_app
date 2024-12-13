import { HttpParams, HttpResponse } from "@angular/common/http";
import { signal } from "@angular/core";
import { PaginationResults } from "../_models/Pagination";
import { UserParams } from "../_models/UserParams";

export function getParams(userparams: UserParams): HttpParams {
    let params = new HttpParams();
    if (userparams.pageNumber && userparams.pageSize) {
        params = params.append("pageNumber", userparams.pageNumber);
        params = params.append("PageSize", userparams.pageSize);
    }
    if (userparams.gender)
        params = params.append("gender", userparams.gender);
    if (userparams.minAge)
        params = params.append("minAge", userparams.minAge);
    if (userparams.maxAge)
        params = params.append("maxAge", userparams.maxAge);
    return params;
}
  export function paginatedResp<T>(response: HttpResponse<T>,paginatedResultSignal : ReturnType<typeof signal<PaginationResults<T[]>|null>>) {
    paginatedResultSignal.set({
        items: response.body as T[],
        Pagination: JSON.parse(response.headers.get("Pagination")!)
    })
}