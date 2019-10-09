import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { DfService } from './df.service';
import {Sort} from '@angular/material/sort';
import { DocumentFile } from './df-file';

@Component({
    selector: 'app-documentfile',
    templateUrl: './df.component.html',
    styleUrls: ['./df.component.css']
})

export class DfComponent implements OnInit {
    @Input() input: string;

    public dfList;

    sortedData: DocumentFile[];

    constructor(private dfServ: DfService, private elementRef:ElementRef) { 
        this.input = this.elementRef.nativeElement.getAttribute('input');
    }

    ngOnInit() {
        console.log(this.input)
        this.loadGithubRepos();
    }

    public loadGithubRepos() {
        this.dfServ.getDocumentFiles(this.input).subscribe(data => {
            this.dfList = data;
            this.sortedData = data as unknown as DocumentFile[];
            console.log('----');
            console.log(this.dfList);
            console.log('----');
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