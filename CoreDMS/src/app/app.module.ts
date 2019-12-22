import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; 
import { AppComponent } from './app.component';

import { DfComponent } from './documentfile/df.component';
import { DfFormComponent } from './documentfile/dfform.component';

import { FormsModule }   from '@angular/forms';

import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {DemoMaterialModule} from './material-module';
import { ConfirmationDialogComponent } from './shared/delete-confirm-dialog/delete-confirm-dialog.component';


@NgModule({
  declarations: [
    // AppComponent,
    DfComponent,
    DfFormComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatDialogModule,
    MatButtonModule,
    DemoMaterialModule,
    FormsModule
  ],
  entryComponents: [
    ConfirmationDialogComponent
  ],
  providers: [],
  bootstrap: [DfComponent, DfFormComponent]
})
export class AppModule { }
