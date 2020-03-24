import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; 
import { AppComponent } from './app.component';
import { IndexComponent } from './index/index.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';

import { DfComponent } from './documentfile/df.component';
import { DfFormComponent } from './documentfile/dfform.component';

import { FormsModule, ReactiveFormsModule }   from '@angular/forms';
import { RouterModule } from '@angular/router';

import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {DemoMaterialModule} from './material-module';
import { ConfirmationDialogComponent } from './shared/delete-confirm-dialog/delete-confirm-dialog.component';
import { DetailComponent } from './detail/detail.component';
import { IndexService } from './index/index.service';
import { DocumentComponent } from './document/document.component';
import { DocumentService } from './document/document.service';
import { MatFormFieldModule, MatInputModule, MatDatepickerModule, MatNativeDateModule, MatSelectModule } from '@angular/material';


@NgModule({
  declarations: [
    AppComponent,
    DfComponent,
    DfFormComponent,
    IndexComponent,
    DetailComponent,
    DocumentComponent,
    NavBarComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,        // <----- import(must)
    MatNativeDateModule,        // <----- import for date formating(optional)
    MatSelectModule,
    DemoMaterialModule,
    FormsModule,
    ReactiveFormsModule,    
    RouterModule.forRoot([
      { path: '', component: IndexComponent },
      { path: 'home/all', component: IndexComponent },
      { path: 'file2/:fileId', component: DocumentComponent },
      { path: 'tags', component: IndexComponent },
      { path: 'files', component: IndexComponent },
      { path: 'search', component: IndexComponent },
      { path: 'upload', component: IndexComponent },
      { path: 'uploads', component: IndexComponent },
      { path: 'erros', component: IndexComponent },
      { path: 'products/:productId', component: DetailComponent },
    ])
  ],
  entryComponents: [
    ConfirmationDialogComponent
  ],
  providers: [ IndexService, DocumentService ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
