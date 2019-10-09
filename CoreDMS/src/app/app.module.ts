import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { HttpClientModule } from '@angular/common/http'; 
import { AppComponent } from './app.component';

import { DfComponent } from './documentfile/df.component';

import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {DemoMaterialModule} from './material-module';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    DemoMaterialModule,
  ],
  declarations: [
    DfComponent
  ],
  bootstrap: [DfComponent],
  providers: [],
})
export class AppModule { }
