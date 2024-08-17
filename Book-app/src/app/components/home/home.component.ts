import { Component, OnInit, ChangeDetectionStrategy, ChangeDetectorRef } from '@angular/core';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit {
  books: Book[] = [];

  constructor(private bookService: BookService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.getBooks().subscribe(books => {
      this.books = books;
      this.cdr.markForCheck();
    });
  }

  deleteBook(id: number): void {
    console.log(`Deleting book with id: ${id}`);
    this.bookService.deleteBook(id).subscribe(
      () => {
        console.log(`Before deletion: ${this.books.length} books`);
        this.books = this.books.filter(book => book.id !== id);
        console.log(`After deletion: ${this.books.length} books`);
        this.cdr.markForCheck();
      },
      (error) => {
        console.error('Error deleting book:', error);
      }
    );
  }
}
