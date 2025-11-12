import { ApplicationRef, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { BrowserModule, createApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { createCustomElement } from '@angular/elements';
import { AtmShellComponent } from './app/features/atm-shell/atm-shell.component';

createApplication({
  providers: [
    importProvidersFrom(BrowserModule),
    provideHttpClient(),
    provideZoneChangeDetection({ eventCoalescing: true })
  ]
})
  .then((appRef: ApplicationRef) => {
    const atmElement = createCustomElement(AtmShellComponent, {
      injector: appRef.injector
    });
    customElements.define('atm-root', atmElement);
  })
  .catch((err: unknown) => console.error(err));
