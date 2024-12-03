import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataListBoxComponent } from './data-list-box.component';

describe('DataListBoxComponent', () => {
  let component: DataListBoxComponent;
  let fixture: ComponentFixture<DataListBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataListBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DataListBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
