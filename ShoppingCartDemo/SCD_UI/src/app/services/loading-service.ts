import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private requestCount = 0;
  public isLoading = new BehaviorSubject<boolean>(false);

  show() {
    this.requestCount++;
    this.isLoading.next(true);
  }

  hide() {
    this.requestCount--;
    if (this.requestCount <= 0) {
      this.requestCount = 0;
      this.isLoading.next(false);
    }
  }
}
