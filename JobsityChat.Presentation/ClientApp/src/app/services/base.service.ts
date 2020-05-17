import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { BaseResponseModel } from '../models/baseResponseModel';

@Injectable({
  providedIn: 'root'
})
export class BaseService {
  url: string;
  options;

  constructor(private httpClient: HttpClient) {
    this.url = 'https://localhost:44340/api';
    const currentUser = this.getCurrentUser<string>();


    if (currentUser) {
      this.options = {
        headers: new HttpHeaders().append('Authorization', 'Bearer ' + currentUser)
      };
    }
  }

  getCurrentUser<T>(): T {
    return this.getLocalStorage<T>('currentUser');
  }

  getLocalStorage<T>(name): T {
    return JSON.parse(localStorage.getItem(name)) as T;
  }

  setLocalStorage(name, object) {
    const stringfiedObject = JSON.stringify(object);
    return localStorage.setItem(name, stringfiedObject);
  }

  removeLocalStorage(name) {
    return localStorage.removeItem(name);
  }

  public post<T, T2>(item: T2, endpoint: string): Observable<BaseResponseModel<T>> {
    return this.httpClient.post<T>(`${this.url}/${endpoint}`, item, this.options).pipe(
      map((data: any) => {
        const result = data as BaseResponseModel<T>;
        return result;
      })
    );
  }

  public put<T>(item: T, endpoint: string): Observable<T> {
    return this.httpClient.put<T>(`${this.url}/${endpoint}`, item, this.options).pipe(
      map((data: any) => {
        const result = data as BaseResponseModel<T>;
        return result.response;
      })
    );
  }

  public get<T>(id: number, endpoint: string): Observable<T> {
    return this.httpClient.get(`${this.url}/${endpoint}/${id ? id : ''}`, this.options).pipe(
      map((data: any) => {
        const result = data as BaseResponseModel<T>;
        return result.response;
      })
    );
  }

  public getMultipleOptions<T>(options: number[], endpoint: string): Observable<T> {
    return this.httpClient.get(`${this.url}/${endpoint}/${options.join('/')}`, this.options).pipe(
      map((data: any) => {
        const result = data as BaseResponseModel<T>;
        return result.response;
      })
    );
  }

  public getList<T>(
    queryOptions: QueryOptions,
    endpoint: string
  ): Observable<T[]> {
    let address = `${this.url}/${endpoint}`;
    if (queryOptions) {
      address += `?${queryOptions.toQueryString()}`;
    }

    return this.httpClient.get(address, this.options).pipe(
      map((data: any) => {
        const result = data as BaseResponseModel<T[]>;
        return result.response;
      })
    );
  }

  delete(id: number, endpoint: string) {
    return this.httpClient.delete(`${this.url}/${endpoint}/${id}`, this.options);
  }
}

export interface QueryBuilder {
  toQueryMap: () => Map<string, string>;
  toQueryString: () => string;
}

export class QueryOptions implements QueryBuilder {
  public pageNumber: number;
  public pageSize: number;

  constructor() {
    this.pageNumber = 1;
    this.pageSize = 10000;
  }

  toQueryMap() {
    const queryMap = new Map<string, string>();
    queryMap.set('pageNumber', `${this.pageNumber}`);
    queryMap.set('pageSize', `${this.pageSize}`);

    return queryMap;
  }

  toQueryString() {
    let queryString = '';
    this.toQueryMap().forEach((value: string, key: string) => {
      queryString = queryString.concat(`${key}=${value}&`);
    });

    return queryString.substring(0, queryString.length - 1);
  }
}

