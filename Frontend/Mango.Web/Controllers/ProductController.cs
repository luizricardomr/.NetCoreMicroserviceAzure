﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO>? list = new();

            ResponseDTO? response = await _productService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            else
                TempData["error"] = response?.Message;

            return View(list);
        }
        public async Task<IActionResult> ProductCreate(int? ProductId)
        {
            if(ProductId != null)
            {
                ResponseDTO? response = await GetProductDTOById((int)ProductId);

                if (response != null && response.IsSuccess)
                {
                    ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
                    return View(model);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {
            ResponseDTO? response = null;
            string message = "";
            if (ModelState.IsValid)
            {
                if(model.ProductId > 0)
                {
                    response = await _productService.UpdateProductAsync(model);
                    message = "Product updated successfully";
                }
                else
                {
                    model.ProductId = 0;
                    response = await _productService.CreateProductAsync(model);
                    message = "Product created successfully";
                }                    

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = message;
                    return RedirectToAction(nameof(ProductIndex));                    
                }
                else
                    TempData["error"] = response?.Message;
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDTO? response = await GetProductDTOById(ProductId);

            if (response != null && response.IsSuccess)
            {
                ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));                
                return View(model);
            }
            else
                TempData["error"] = response?.Message;

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {
            ResponseDTO? response = await _productService.DeleteProductAsync(model.ProductId.Value);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
                TempData["error"] = response?.Message;
            return View(model);
        }

        private async Task<ResponseDTO> GetProductDTOById(int ProductId)
        {
            return await _productService.GetProductByIdAsync(ProductId);
        }
    }
}