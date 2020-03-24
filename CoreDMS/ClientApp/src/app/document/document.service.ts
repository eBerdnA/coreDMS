import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { DocumentFile } from '../dto/document';
import { HttpHeaders } from '@angular/common/http';

@Injectable()
export class DocumentService {
    constructor(private httpClient: HttpClient) { }

    updateDocument(documentGuid: string, values): Observable<DocumentFile> {
        const httpOptions = {
            headers: new HttpHeaders({
              'Content-Type':  'application/json'
            })
        };
        return this.httpClient.post<DocumentFile>('/api/document/' + documentGuid, values , httpOptions);
    }

    getDocumentByGuid(documentGuid: string): Observable<Array<DocumentFile>> {
        const httpOptions = {
            headers: new HttpHeaders({
              'Content-Type':  'application/json'
            })
        };
        return this.httpClient.get<Array<DocumentFile>>('/api/document/' + documentGuid, httpOptions);
    }
}