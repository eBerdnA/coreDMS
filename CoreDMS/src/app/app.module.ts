import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http'; 
import { AppComponent } from './app.component';

import { DfComponent } from './documentfile/df.component';

@NgModule({
  declarations: [
    // AppComponent,
    DfComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [DfComponent]
})
export class AppModule { }
