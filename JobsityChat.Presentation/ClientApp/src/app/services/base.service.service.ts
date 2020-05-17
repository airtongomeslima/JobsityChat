import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseService {
  url: string;
  options;

  constructor(private httpClient: HttpClient) {
    this.url = 'https://localhost:44340/api/';
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

  public post<T, T2>(item: T2, endpoint: string): Observable<T> {
    return this.httpClient.post<T>(`${this.url}/${endpoint}`, item, this.options).pipe(
      map((data: any) => {
        const result = data as T;
        if (result) {
          return result;
        } else {
          return null;
        }
      })
    );
  }

  public put<T>(item: T, endpoint: string): Observable<T> {
    return this.httpClient.put<T>(`${this.url}/${endpoint}`, item, this.options).pipe(
      map((data: any) => {
        const result = data as T;
        return result;
      })
    );
  }

  public get<T>(id: number, endpoint: string): Observable<T> {
    return this.httpClient.get(`${this.url}/${endpoint}/${id ? id : ''}`, this.options).pipe(
      map((data: any) => {
        const result = data as T;
        return result;
      })
    );
  }

  public getMultipleOptions<T>(options: number[], endpoint: string): Observable<T> {
    console.log('options', endpoint, options);
    return this.httpClient.get(`${this.url}/${endpoint}/${options.join('/')}`, this.options).pipe(
      map((data: any) => {
        const result = data as T;
        return result;
      })
    );
  }

  public getList<T>(
    // queryOptions: helpers.QueryOptions,
    endpoint: string
  ): Observable<T[]> {
    let address = `${this.url}/${endpoint}`;
    // if (queryOptions) {
    //   address += `?${queryOptions.toQueryString()}`;
    // }

    return this.httpClient.get(address, this.options).pipe(
      map((data: any) => {
        const result = data as T[];
        return result;
      })
    );
  }

  delete(id: number, endpoint: string) {
    return this.httpClient.delete(`${this.url}/${endpoint}/${id}`, this.options);
  }
}
