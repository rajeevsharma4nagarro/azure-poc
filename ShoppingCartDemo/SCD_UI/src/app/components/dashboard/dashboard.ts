import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { GlobalService, MockProducts } from '../../services/global.services';
import { ProductCategory } from '../../services/global.services';
import { ProductService } from '../../services/product-service';
import { CartStateService } from '../checkout/cart-items/CartStateService/cart-state-service';

const enumKeys: string[] = Object.keys(ProductCategory).filter(key => isNaN(Number(key)));

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  products: iProduct[] = [];

  categories = enumKeys;
  selectedCategory: string = 'All';

  // Pagination properties
  pageSize: number = 6;
  currentPage: number = 1;
  filteredProducts: iProduct[] = [];//...this.products
  paginatedProducts: iProduct[] = [];
  totalPages: number = 1;
  isAdmin: boolean = false;
  
  constructor(private pService: ProductService, private cdr: ChangeDetectorRef, private cartState: CartStateService
    ,private globalServices: GlobalService
  ) {
    
  }

  ngOnInit() {
    this.globalServices.userProfile$.subscribe({
      next: (profile: any) => {
        this.isAdmin = (profile?.roleName == 'Admin')? true: false;
      },
      error: () => { },
      complete: () => { }
    });
    this.getAllProducts();
  }

  getAllProducts() {
    this.pService.getAllProducts().subscribe({
      next: (res) => {
        this.products = res.result;
        this.filterCategory(this.selectedCategory);
        this.cdr.detectChanges();
      },
      error: () => {

      },
      complete: () => {

      }
    })
  }

  filterCategory(category: string) {
    this.selectedCategory = category;
    this.filteredProducts =
      category === 'All'
        ? this.products
        : this.products.filter((p) => p.category === category);

    this.currentPage = 1; // reset to first page
    this.updatePagination();
  }

  getCategoryCount(category: string): number {
    return this.products.filter((p) => p.category === category).length;
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredProducts.length / this.pageSize);
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedProducts = this.filteredProducts.slice(start, end);
  }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePagination();
  }

  addToCart(product: iProduct) {
    let ci: CartItem = { ...product, qty: 1 };
    this.cartState.addItem(ci);
    alert('Cart updated now.');
  }
}
