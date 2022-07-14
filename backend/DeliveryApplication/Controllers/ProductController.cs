using AutoMapper;
using DeliveryApplication.Dtos;
using DeliveryApplication.Interfaces;
using DeliveryApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        [HttpPost("newProduct")]
        public async Task<IActionResult> AddProduct(ProductDto newProduct)
        {
            if (await uow.ProductRepository.ProductAlreadyExists(newProduct.Name))
                return BadRequest("Product already exists, please try something else.");
            if (AddProductFieldsValidation(newProduct, out string message))
                return BadRequest(message);

            uow.ProductRepository.AddProduct(newProduct.Name, newProduct.Ingredients, newProduct.Price);
            await uow.SaveAsync();
            return StatusCode(201);
        }

        private bool AddProductFieldsValidation(ProductDto newProduct, out string message)
        {
            if (newProduct.Name.Length == 0 || newProduct.Ingredients.Length == 0 || newProduct.Price.ToString().Length == 0)
            {
                message = "All fields must be filled!";
                return true;
            }

            if (newProduct.Price < 0)
            {
                message = "Price must be greater than 0!";
                return true;
            }

            message = "";
            return false;
        }

        [HttpGet("allProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                List<Product> products = await uow.ProductRepository.GetAllProducts();
                List<ProductDto> productsDto = mapper.Map<List<ProductDto>>(products);

                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
