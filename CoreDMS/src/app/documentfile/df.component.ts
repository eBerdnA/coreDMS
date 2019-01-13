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
    model = new FileToDocumentFile('');
    error = "";
    submitted = false;
 
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

export class FileToDocumentFile {

    constructor(
      public documentid: string
    ) {  }
  
}