import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, tap } from "rxjs/operators";
import { Observable, of } from "rxjs";
import { DocumentFile } from './df-file';
import { HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})

export class DfService { 
    constructor(private httpClient: HttpClient) { }

    addDocumentToFile(fileid: string, documentid: string): Observable<DocumentFile> {
        const httpOptions = {
            headers: new HttpHeaders({
              'Content-Type':  'application/json'
            })
        };
        return this.httpClient.post<DocumentFile>('/add/documentfileid', {fileid, documentid}, httpOptions);
    }

    removeDocumentFromFile(fileid: string, documentid: string): Observable<DocumentFile> {
      const httpOptions = {
          headers: new HttpHeaders({
            'Content-Type':  'application/json'
          })
      };
      return this.httpClient.post<DocumentFile>('/remove/documentfileid', {fileid, documentid}, httpOptions);
    }

    private handleError<T> (serviceName = '', operation = 'operation', result = {} as T) {

        return (error: HttpErrorResponse): Observable<T> => {
          // TODO: send the error to remote logging infrastructure
          console.error(error); // log to console instead
    
          const message = (error.error instanceof ErrorEvent) ?
            error.error.message :
           `server returned code ${error.status} with body "${error.error}"`;
    
          // TODO: better job of transforming error for user consumption
        //   this.messageService.add(`${serviceName}: ${operation} failed: ${message}`);
    
          // Let the app keep running by returning a safe result.
          return of( result );
        };
    
      }

    getDocumentFiles(fileid: string): Observable<DocumentFile> {
        const httpOptions = {
            headers: new HttpHeaders({
              'Content-Type':  'application/json'
            })
          };

        var result = this.httpClient.post('/get/documentfileid', fileid, httpOptions);
        result.subscribe(results => console.log(results));
        return result.pipe(
                map((item: any) => item.map(p => <DocumentFile> {
                    filename: p.filename,
                    id: p.id,
                    createdAt: p.createdAt,
                    updatedAt: p.updatedAt,
                    documentDate: p.documentDate,
                    hash: p.hash
                })));
    }
}