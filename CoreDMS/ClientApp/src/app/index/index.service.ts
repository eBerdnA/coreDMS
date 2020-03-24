import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { DocumentFile } from '../dto/document';
import { HttpHeaders } from '@angular/common/http';

@Injectable()
export class IndexService {
    constructor(private httpClient: HttpClient) { }

    getNewDocuments(): Observable<Array<DocumentFile>> {
        const httpOptions = {
            headers: new HttpHeaders({
              'Content-Type':  'application/json'
            })
        };
        return this.httpClient.get<Array<DocumentFile>>('/api/documents/new', httpOptions);
    }
}