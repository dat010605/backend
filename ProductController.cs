using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// Định nghĩa lớp model Product.
// Thuộc tính 'required' đảm bảo Name phải có giá trị.
public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    // Dữ liệu giả để minh họa. Trong thực tế, bạn sẽ dùng database.
    private static List<Product> _products = new List<Product>
    {
        new Product { Id = 1, Name = "Laptop", Price = 1500 },
        new Product { Id = 2, Name = "Mouse", Price = 25 }
    };

    // GET: api/Product - Lấy tất cả sản phẩm
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        return Ok(_products);
    }

    // GET: api/Product/{id} - Lấy sản phẩm theo ID
    [HttpGet("{id}")]
    public ActionResult<Product> GetProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    // POST: api/Product - Tạo sản phẩm mới
    [HttpPost]
    public ActionResult<Product> AddProduct(Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/Product/{id} - Cập nhật sản phẩm
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, Product updatedProduct)
    {
        if (id != updatedProduct.Id)
        {
            return BadRequest();
        }

        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;

        return NoContent();
    }

    // DELETE: api/Product/{id} - Xóa sản phẩm
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        _products.Remove(product);

        return NoContent();
    }
}