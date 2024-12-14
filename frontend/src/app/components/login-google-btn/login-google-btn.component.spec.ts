import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginGoogleBtnComponent } from './login-google-btn.component';

describe('LoginGoogleBtnComponent', () => {
  let component: LoginGoogleBtnComponent;
  let fixture: ComponentFixture<LoginGoogleBtnComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoginGoogleBtnComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginGoogleBtnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
