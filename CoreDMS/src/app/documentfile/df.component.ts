import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { DfService } from './df.service';

@Component({
    selector: 'app-documentfile',
    templateUrl: './df.component.html',
    styleUrls: ['./df.component.css']
})

export class DfComponent implements OnInit {
    @Input() input: string;

    public dfList;

    constructor(private dfServ: DfService, private elementRef:ElementRef) { 
        this.input = this.elementRef.nativeElement.getAttribute('input');
    }

    ngOnInit() {
        console.log(this.input)
        this.loadGithubRepos();
    }

    public loadGithubRepos() {
        this.dfServ.getDocumentFiles(this.input).subscribe((data) => {
            this.dfList = data;
            console.log(this.dfList);
        });
    }
}