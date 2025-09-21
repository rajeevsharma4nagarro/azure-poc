import { Component, OnInit } from '@angular/core';
import { ProductCategory } from '../../services/global.services';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../services/product-service';
import { Router } from '@angular/router';

const enumKeys: string[] = Object.keys(ProductCategory).filter(key => isNaN(Number(key)));

@Component({
  selector: 'app-add-product',
  standalone: false,
  templateUrl: './add-product.html',
  styleUrl: './add-product.css'
})
export class AddProduct implements OnInit {
  productForm: FormGroup;
  public enumKeys = enumKeys
  selectedFile: File | string = "";
  constructor(private fb: FormBuilder, private service: ProductService, private router: Router) {
    this.productForm = this.fb.group({
      category: ['', Validators.required],
      name: ['', Validators.required],
      price: ['', [Validators.required, Validators.min(1), Validators.max(10000)]],
      description: ['', Validators.required],
      image: [null] // For image file
    });
  }

  ngOnInit(): void {
    
  }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  onSubmit(): void {
    if (this.productForm.valid) {
      const formData = new FormData();
      formData.append('name', this.productForm.value.name);
      formData.append('category', this.productForm.value.category);
      formData.append('price', this.productForm.value.price);
      formData.append('description', this.productForm.value.description);
      formData.append('image', this.selectedFile);


      this.service.addProduct(formData).subscribe({
        next: (resp) => {
          console.log('Produt added.');
          alert("Product added.");
          this.router.navigate(['/home']);
        },
        error: (err) => {
          alert(err)
        },
        complete: () => {

        }
      });
      
    } else {
      console.log('Form is invalid');
    }
  }
}
