import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayMatchComponent } from './play-match.component';

describe('PlayMatchComponent', () => {
  let component: PlayMatchComponent;
  let fixture: ComponentFixture<PlayMatchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlayMatchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlayMatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
