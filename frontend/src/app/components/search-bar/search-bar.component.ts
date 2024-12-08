import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { debounceTime, Subject } from 'rxjs';

@Component({
  selector: 'app-search-bar',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.css'
})
export class SearchBarComponent implements OnInit {
  private typingTimer = new Subject<string>();
  reference = "";

  constructor (private route: Router, private activedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activedRoute.queryParams.subscribe(params => {
      const reference = params['reference'];
      this.reference = reference;
    })

    this.typingTimer.pipe(debounceTime(500)).subscribe(value => {
      this.route.navigate(['search'], { queryParams: { reference: value } })
    })
  }

  search() {
    this.route.navigate(['search'], { queryParams: { reference: this.reference } })
  }

  onKeyup(event: any): void {
    let value: string = event.target.value;
    value = value.trim();
    this,this.typingTimer.next(value);
  }
}
