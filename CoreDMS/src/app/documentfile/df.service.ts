import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, tap } from "rxjs/operators";
import { Observable } from "rxjs";
import { DocumentFile } from './df-file';
import { HttpHeaders } from '@angular/common/http';


@Injectable({
    providedIn: 'root'
})

export class DfService { 
    constructor(private httpClient: HttpClient) { }

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