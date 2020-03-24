import { Component, OnInit } from '@angular/core';
import { IndexService } from './index.service';
import { Sort } from '@angular/material/sort';
import { DocumentFile } from '../dto/document';

@Component({
    selector: 'index-list',
    templateUrl: './index.component.html',
    styleUrls: ['./index.component.css']
})

export class IndexComponent implements OnInit {
    public documentList;
    sortedData: DocumentFile[];

    constructor(private indexService: IndexService) {

    }

    ngOnInit() {
        console.log('in ngOnInit');
        this.indexService.getNewDocuments().subscribe((data) => {
            this.documentList = data;
            this.sortedData = data as unknown as DocumentFile[];
            console.log(this.documentList);
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