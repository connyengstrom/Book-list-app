import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BookService } from '../../services/book.service';

@Component({
  selector: 'app-add-edit-book',
  templateUrl: './add-edit-book.component.html',
  styleUrls: ['./add-edit-book.component.css']
})
export class AddEditBookComponent implements OnInit {
  bookForm: FormGroup;
  isEditMode: boolean = false;
  bookId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private bookService: BookService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.bookForm = this.fb.group({
      id: [0],
      title: ['', Validators.required],
      author: ['', Validators.required],
      genre: ['', Validators.required],
      publishedDate: ['', Validators.required] // Ensure this expects a string
    });
  }

  ngOnInit(): void {
    this.bookId = this.route.snapshot.params['id'];
    this.isEditMode = !!this.bookId;

    if (this.isEditMode && this.bookId) {
      this.bookService.getBook(this.bookId).subscribe((book) => {
        // Ensure the date is in the correct format
        if (book.publishedDate) {
          const formattedDate = this.formatDate(book.publishedDate);
          this.bookForm.patchValue({
            ...book,
            publishedDate: formattedDate // Patch as a string
          });
        } else {
          this.bookForm.patchValue(book);
        }
      });
    }
  }

  private formatDate(date: Date): string {
    const parsedDate = new Date(date);
    return parsedDate.toISOString().substring(0, 10); // Format to 'YYYY-MM-DD'
  }

  onSubmit(): void {
    if (this.bookForm.invalid) {
      return;
    }

    const bookData = this.bookForm.value;

    if (this.isEditMode && this.bookId) {
      bookData.id = this.bookId;
      this.bookService.updateBook(this.bookId, bookData).subscribe(
        () => {
          this.router.navigate(['/']);
        },
        (error) => {
          console.error('Error updating book:', error);
        }
      );
    } else {
      this.bookService.createBook(bookData).subscribe(
        () => {
          this.router.navigate(['/']);
        },
        (error) => {
          console.error('Error creating book:', error);
        }
      );
    }
  }
}
