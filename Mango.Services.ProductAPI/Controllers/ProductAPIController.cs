using AutoMapper;
using Mango.Services.ProductAPI.Dtos;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public ProductAPIController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<ResponseDto> GetAll()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseDto> GetById(int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    _responseDto.Message = $"No data with id {id}";
                }
                else
                {
                    _responseDto.Result = _mapper.Map<ProductDto>(product);
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Post([FromBody] ProductCreateDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "The coupon is null";
                }
                else
                {
                    Product coupon = _mapper.Map<Product>(productDto);
                    _context.Products.Add(coupon);
                    await _context.SaveChangesAsync();

                    _responseDto.Result = coupon;
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Put([FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "The product is null";
                }
                else
                {
                    Product product = _mapper.Map<Product>(productDto);
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    _responseDto.Result = product;
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                Product? product = _context.Products.FirstOrDefault(p => p.ProductId == id);

                if (product == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"The product with id {id} doesn't exists";
                }
                else
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    _responseDto.Result = null;
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }
    }
}
