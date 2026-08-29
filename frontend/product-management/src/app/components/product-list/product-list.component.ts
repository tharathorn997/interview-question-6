import { Component, OnInit, Directive, ElementRef, Input, OnChanges, inject, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import JsBarcode from 'jsbarcode';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Directive({
  selector: '[appBarcode]',
  standalone: true
})
export class BarcodeDirective implements OnChanges {
  @Input('appBarcode') value: string = '';

  constructor(private el: ElementRef) {}

  ngOnChanges() {
    if (this.value) {
      try {
        JsBarcode(this.el.nativeElement, this.value, {
          format: 'CODE39',
          height: 40,
          width: 1.2,
          displayValue: false,
          margin: 0
        });
      } catch (e) {
        console.error('Error generating barcode', e);
      }
    }
  }
}

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [FormsModule, BarcodeDirective],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private cdr = inject(ChangeDetectorRef);

  products: Product[] = [];
  productCode: string = '';
  errorMessage: string = '';
  showConfirmDialog: boolean = false;
  productToDelete: Product | null = null;
  isLoading: boolean = false;

  private readonly codePattern = /^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$/;

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading = true;
    this.productService.getAll().subscribe({
      next: (data) => {
        this.products = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'ไม่สามารถโหลดข้อมูลสินค้าได้';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onInputChange(): void {
    let value = this.productCode.toUpperCase().replace(/[^A-Z0-9]/g, '');
    if (value.length > 16) value = value.substring(0, 16);
    const parts = value.match(/.{1,4}/g) || [];
    this.productCode = parts.join('-');
    this.errorMessage = '';
    this.cdr.detectChanges();
  }

  addProduct(): void {
    const code = this.productCode.trim();
    if (!code) {
      this.errorMessage = 'กรุณากรอกรหัสสินค้า';
      this.cdr.detectChanges();
      return;
    }

    if (!this.codePattern.test(code)) {
      this.errorMessage = 'รหัสสินค้าต้องเป็นตัวเลขและตัวอักษรพิมพ์ใหญ่ 16 หลัก (XXXX-XXXX-XXXX-XXXX)';
      this.cdr.detectChanges();
      return;
    }

    this.errorMessage = '';
    this.isLoading = true;
    this.cdr.detectChanges();

    this.productService.create({ productCode: code }).subscribe({
      next: (newProduct) => {
        this.productCode = '';
        this.errorMessage = '';
        this.isLoading = false;
        this.products = [...this.products, newProduct];
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 409) {
          this.errorMessage = 'รหัสสินค้านี้มีอยู่ในระบบแล้ว';
        } else {
          this.errorMessage = 'เกิดข้อผิดพลาดในการเพิ่มข้อมูล';
        }
        this.cdr.detectChanges();
      }
    });
  }

  confirmDelete(product: Product): void {
    this.productToDelete = product;
    this.showConfirmDialog = true;
    this.cdr.detectChanges();
  }

  cancelDelete(): void {
    this.showConfirmDialog = false;
    this.productToDelete = null;
    this.cdr.detectChanges();
  }

  executeDelete(): void {
    if (!this.productToDelete) return;
    const id = this.productToDelete.id;

    this.productService.delete(id).subscribe({
      next: () => {
        this.products = this.products.filter(p => p.id !== id);
        this.showConfirmDialog = false;
        this.productToDelete = null;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'เกิดข้อผิดพลาดในการลบข้อมูล';
        this.showConfirmDialog = false;
        this.cdr.detectChanges();
      }
    });
  }

  getBarcodeValue(productCode: string): string {
    return productCode.replace(/-/g, '');
  }
}
