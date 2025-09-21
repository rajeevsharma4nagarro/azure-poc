import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-confirmation',
  standalone: false,
  templateUrl: './confirmation.html',
  styleUrl: './confirmation.css'
})
export class Confirmation {
  orderId: string|null = "";

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id'); // One-time read
    // OR subscribe for changes
    this.route.paramMap.subscribe(params => {
      this.orderId = params.get('id');
    });
  }
}
