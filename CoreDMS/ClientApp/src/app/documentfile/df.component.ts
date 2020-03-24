import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { DocumentFile } from './df-file';

import { DfService } from './df.service';
import { MatDialog } from '@angular/material';
import { ConfirmationDialogComponent } from '../shared/delete-confirm-dialog/delete-confirm-dialog.component';

@Component({
    selector: 'app-documentfile',
    templateUrl: './df.component.html',
    styleUrls: ['./df.component.css']
})

export class DfComponent implements OnInit {
    @Input() input: string;

    public dfList;
    model = new FileToDocumentFile('');
    error = "";
    submitted = false;

    sortedData: DocumentFile[];


    constructor(private dfServ: DfService, private elementRef:ElementRef, public dialog: MatDialog) { 
        this.input = this.elementRef.nativeElement.getAttribute('input');
    }

    openDialog(documentId): void {
        const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
          width: '350px',
          data: "Do you confirm the removal of this document from this file?"
        });
    
        dialogRef.afterClosed().subscribe(result => {
          if(result) {
            console.log('Yes clicked');
            this.dfServ.removeDocumentFromFile(documentId, this.input).subscribe(
                (data) => {                
                    console.log(data);            
                    this.loadGithubRepos();
                },
                error => {
                    console.log(error.error)
                    this.error = error.error;
                }
            );
          } else {
            console.log('No clicked');
            console.log(documentId);
          }
        });
      }
 
    onSubmit() { 
        this.submitted = true; 
        this.error = "";
        console.log(this.model);
        this.dfServ.addDocumentToFile(this.model.documentid,this.input).subscribe(
            (data) => {                
                console.log(data);            
                this.loadGithubRepos();
                this.model  = new FileToDocumentFile('');
            },
            error => {
                console.log(error.error)
                this.error = error.error;
            }
            );
    }

    delete(item) {
        this.dfServ.removeDocumentFromFile(item, this.input).subscribe(
            (data) => {                
                console.log(data);            
                this.loadGithubRepos();
            },
            error => {
                console.log(error.error)
                this.error = error.error;
            }
        );
    }

    ngOnInit() {
        console.log(this.input)
        this.loadGithubRepos();
    }

    public loadGithubRepos() {
        this.dfServ.getDocumentFiles(this.input).subscribe((data) => {
            this.dfList = data;
            this.sortedData = data as unknown as DocumentFile[];
            console.log(this.dfList);
        });
    }

    sortData(sort: Sort) {
        console.log("sorting");
        console.log(this.sortedData);
        const data = this.sortedData;
        if (!sort.active || sort.direction === '') {
            this.sortedData = data;
            return;
        }

        this.sortedData = data.sort((a, b) => {
            const isAsc = sort.direction === 'asc';
            switch (sort.active) {
              case 'filename': return compare(a.filename, b.filename, isAsc);
              case 'hash': return compare(a.hash, b.hash, isAsc);
              case 'documentDate': return compare(a.documentDate, b.documentDate, isAsc);
              case 'updatedAt': return compare(a.updatedAt, b.updatedAt, isAsc);
              default: return 0;
            }
        });
    }
}

function compare(a: number | string, b: number | string, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
  }

export class FileToDocumentFile {

    constructor(
      public documentid: string
    ) {  }
  
}