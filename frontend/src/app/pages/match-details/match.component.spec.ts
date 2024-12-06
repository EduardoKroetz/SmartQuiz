import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MatchDetailsComponent } from './match.component';

describe('MatchDetailsComponent', () => {
  let component: MatchDetailsComponent;
  let fixture: ComponentFixture<MatchDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatchDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MatchDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
