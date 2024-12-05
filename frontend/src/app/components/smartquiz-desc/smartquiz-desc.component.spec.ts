import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmartquizDescComponent } from './smartquiz-desc.component';

describe('SmartquizDescComponent', () => {
  let component: SmartquizDescComponent;
  let fixture: ComponentFixture<SmartquizDescComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SmartquizDescComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SmartquizDescComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
