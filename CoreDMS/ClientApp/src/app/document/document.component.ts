import { ActivatedRoute } from '@angular/router';
import { OnInit, Component } from '@angular/core';
import { DocumentService } from './document.service';
import { DocumentFile } from '../dto/document';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import {MAT_MOMENT_DATE_FORMATS, MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';

@Component({
    selector: 'document-details',
    templateUrl: './document.component.html',
    styleUrls: ['./document.component.css'],
    providers: [
        // `MomentDateAdapter` and `MAT_MOMENT_DATE_FORMATS` can be automatically provided by importing
        // `MatMomentDateModule` in your applications root module. We provide it at the component level
        // here, due to limitations of our example generation script.
        {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
        {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS},
        {provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true }}
      ]
})

export class DocumentComponent implements OnInit {
    documentId;
    document: DocumentDTO.RootObject;
    documentDataForm = this.fb.group({
        documentId: new FormControl(''),
        documentTitle: new FormControl(''),
        documentTags: new FormControl(''),
        documentNote: new FormControl(''),
        documentState: ['', Validators.required],
        documentDate: new FormControl('')

    });
    constructor(
        private documentService: DocumentService, 
        private route: ActivatedRoute, 
        private fb: FormBuilder
    ) {
    }

    onSubmit() {        
        console.warn(this.documentDataForm.value);
        console.warn(this.documentDataForm.value['documentId']);
        this.documentService.updateDocument(this.documentDataForm.value['documentId'],this.documentDataForm.value).subscribe(
            (data) => {                
                console.log(data);
                this.loadData();
            },
            error => {
                console.log(error.error)
                // this.error = error.error;
            }
            );
    }

    loadData() {
        this.documentService.getDocumentByGuid(this.documentId).subscribe((data) => {
            this.document = data as unknown as DocumentDTO.RootObject;
            this.documentDataForm.setValue({
                documentTitle: this.document.file.title,
                documentId: this.document.file.id,
                documentTags: this.document.tags,
                documentNote: this.document.file.note,
                documentState: this.document.file.state,
                documentDate: new Date(this.document.fileDate)
            })
            this.documentDataForm.controls['documentState'].setValue(this.document.file.state.toString());
            console.log(this.document);
        });
    }

    ngOnInit() {
        this.route.paramMap.subscribe(params => {
            this.documentId = params.get('fileId');
            this.loadData(); 
        });
    }
}