// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using APICourse.Models; // Use your namespace here

// namespace APICourse.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class ProductsController : ControllerBase
//     {
//         private readonly ApicourseContext _context;

//         public ProductsController(ApicourseContext context)
//         {
//             _context = context;
//         }

//         // GET: api/products
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
//         {
//             // Return all products from the database
//             return await _context.Products.ToListAsync();
//         }

//         // Optional: You can add more methods (like Get by ID, Create, Update, Delete)
//     }
// }
using Microsoft.AspNetCore.Mvc;
using APICourse.Repositories;
using APICourse.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APICourse.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository; //tao thang nay la de luu lai productRepository va data khong anh huong
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync(); //product nay co all data tu model
            //LINQ to Object 
            var productDtos = products.Select(p => new ProductDto //Lambda expression to map Product to ProductDto
            { //peek
                PId = p.PId,
                ProductName = p.ProductName,
                ProductPrice = p.ProductPrice,
                ProductDescription = p.ProductDescription,
                ProductCategory = p.ProductCategory,
                StockQuantity = p.StockQuantity,
            }).ToList();
            return Ok(productDtos); //200 neu nhu transfer thanh cong, productDtos
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ProductDto>> GetProductById(int id )
        // {
        //     var product = await _productRepository.GetProductByIdAsync(id);

        //     if (product == null)
        //     {
        //         return NotFound(); // Trả về lỗi 404 nếu sản phẩm không tồn tại
        //     }

        //     var productDto = new ProductDto
        //     {
        //         PId = product.PId,
        //         ProductName = product.ProductName,
        //         ProductPrice = product.ProductPrice,
        //         ProductDescription = product.ProductDescription,
        //         ProductCategory = product.ProductCategory,
        //         StockQuantity = product.StockQuantity,
        //         DateAdded = product.DateAdded
        //     };

        //     return Ok(productDto); // Trả về sản phẩm dưới dạng ProductDto
        // }

        // GET: api/products/{id}?proCate={productCategory}
        [HttpGet("{id}")] 
        public async Task<ActionResult<ProductDto>> GetProductById(int id, [FromQuery] string proCate)
        {
            // Nếu có query tham số proCate, tìm theo cả id và ProductCategory
            var productQuery = await _productRepository.GetProductByIdAsync(id);

            if (productQuery == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu sản phẩm không tồn tại
            }

            // Nếu có ProductCategory trong query, kiểm tra xem sản phẩm có cùng loại không
            if (!string.IsNullOrEmpty(proCate) && productQuery.ProductCategory != proCate)
            {
                return NotFound(); // Nếu ProductCategory không trùng khớp, trả về 404
            }

            // Nếu tìm thấy sản phẩm hợp lệ hoặc đúng loại
            var productDto = new ProductDto
            {
                PId = productQuery.PId,
                ProductName = productQuery.ProductName,
                ProductPrice = productQuery.ProductPrice,
                ProductDescription = productQuery.ProductDescription,
                ProductCategory = productQuery.ProductCategory,
                StockQuantity = productQuery.StockQuantity,
                DateAdded = productQuery.DateAdded
            };

            return Ok(productDto); // Trả về sản phẩm dưới dạng ProductDto
        }


        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            if (!ModelState.IsValid) // Kiem tra xem productDto co hop le khong, data type, bo trong,..
            {
                return BadRequest(ModelState);
            }

            var product = new Product
            {
                ProductName = productDto.ProductName,
                ProductPrice = productDto.ProductPrice,
                ProductDescription = productDto.ProductDescription,
                ProductCategory = productDto.ProductCategory,
                StockQuantity = productDto.StockQuantity,
                DateAdded = productDto.DateAdded
            };

            await _productRepository.CreateProductAsync(product);

            return CreatedAtAction(nameof(GetProductById), new { id = product.PId }, product); //no se tao 1 action, no se lay tat ca id co trong databse, sau do no tu tao ra 1 thang khong trung
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto) 
        {
            if (id != productDto.PId)
            {
                return BadRequest();
            }

            var existingProduct = await _productRepository.GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = productDto.ProductName;
            existingProduct.ProductPrice = productDto.ProductPrice;
            existingProduct.ProductDescription = productDto.ProductDescription;
            existingProduct.ProductCategory = productDto.ProductCategory;
            existingProduct.StockQuantity = productDto.StockQuantity;
            existingProduct.DateAdded = productDto.DateAdded;

            await _productRepository.UpdateProductAsync(existingProduct);

            return Ok(existingProduct);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProductAsync(id);

            return Ok(new { message = "Product deleted successfully"});
        }


//  [HttpPost]
//         public async Task<ActionResult<ProductDto>> CreateProduct2(ProductDto productDto)
//         {
//             if (!ModelState.IsValid) // Kiem tra xem productDto co hop le khong, data type, bo trong,..
//             {
//                 return BadRequest(ModelState);
//             }

//             var product = new Product
//             {
//                 ProductName = productDto.ProductName,
//                 ProductPrice = productDto.ProductPrice,
//                 ProductDescription = productDto.ProductDescription,
//                 ProductCategory = productDto.ProductCategory,
//                 StockQuantity = productDto.StockQuantity,
//                 DateAdded = productDto.DateAdded
//             };

//             await _productRepository.CreateProductAsynccc(product);

//             return CreatedAtAction(nameof(GetProductById), new { id = product.PId }, product); //no se tao 1 action, no se lay tat ca id co trong databse, sau do no tu tao ra 1 thang khong trung
//         }

    }
    
}
