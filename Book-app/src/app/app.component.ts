import { Component, AfterViewInit, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit {
  title = 'my-angular-routing-project';
  isDarkMode = false;

  @ViewChild('navbarToggler') navbarToggler!: ElementRef;
  @ViewChild('navbarCollapse') navbarCollapse!: ElementRef;

  constructor(public authService: AuthService, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    this.initializeNavbarToggler();
  }

  initializeNavbarToggler(): void {
    if (this.navbarToggler && this.navbarCollapse) {
      this.renderer.listen(this.navbarToggler.nativeElement, 'click', () => {
        const isCollapsed = this.navbarCollapse.nativeElement.classList.contains('show');
        if (isCollapsed) {
          this.renderer.removeClass(this.navbarCollapse.nativeElement, 'show');
        } else {
          this.renderer.addClass(this.navbarCollapse.nativeElement, 'show');
        }
      });
    }
  }

  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    document.body.classList.toggle('dark-mode', this.isDarkMode);
  }

  logout(): void {
    this.authService.logout();
  }
}
