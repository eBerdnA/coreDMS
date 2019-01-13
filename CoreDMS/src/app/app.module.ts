import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; 
import { AppComponent } from './app.component';

import { DfComponent } from './documentfile/df.component';
import { DfFormComponent } from './documentfile/dfform.component';

import { FormsModule }   from '@angular/forms';

@NgModule({
  declarations: [
    // AppComponent,
    DfComponent,
    DfFormComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [DfComponent, DfFormComponent]
})
export class AppModule { }
