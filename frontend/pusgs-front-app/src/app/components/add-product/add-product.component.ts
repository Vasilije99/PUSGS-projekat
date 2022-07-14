import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Product } from 'src/app/model/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  addProductForm!: FormGroup;
  newProduct!: Product;

  constructor(private productService: ProductService, private fb: FormBuilder) { }

  ngOnInit() {
    this.createAddProductForm();
  }

  createAddProductForm() {
    this.addProductForm = this.fb.group({
      name: [null, Validators.required],
      price: [null, [Validators.required, Validators.pattern(/^[1-9]\d*$/)]],
      ingredients: [null, Validators.required]
    })
  }

  onSubmit() {
    if(this.addProductForm.valid) {
      this.productService.addProduct(this.productData()).subscribe(() => {
        alert("Product successfully added");
        window.location.reload();
      }, error => {
        alert(error);
      })
    }
  }

  productData(): Product {
    return this.newProduct = {
      name: this.name.value,
      price: this.price.value,
      ingredients: this.ingredients.value
    }
  }

  get name() {
    return this.addProductForm.get('name') as FormControl;
  }

  get price() {
    return this.addProductForm.get('price') as FormControl;
  }

  get ingredients() {
    return this.addProductForm.get('ingredients') as FormControl;
  }

}
