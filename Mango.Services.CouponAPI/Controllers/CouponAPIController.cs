using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Azure.Core.HttpHeader;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private ResponseDto _responseDto;

        public CouponAPIController(AppDbContext context, IMapper mapper)
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
                var coupons = await _context.Coupons.ToListAsync();
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
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
                Coupon? coupon = await _context.Coupons.FirstOrDefaultAsync(p => p.CouponId == id);

                if (coupon == null)
                {
                    _responseDto.Message = $"No data with id {id}";
                }
                else
                {
                    _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }
        
        [HttpGet]
        [Route("GetByCode/{code}")]
        public async Task<ResponseDto> GetByCode(string code)
        {
            try
            {
                if(string.IsNullOrEmpty(code))
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "The code is null";
                }
                else
                {
                    Coupon? coupon = await _context.Coupons.FirstOrDefaultAsync(p => EF.Functions.Collate(p.CouponCode, "Latin1_General_CI_AS") == code);

                    if (coupon == null)
                    {
                        _responseDto.Message = $"No data with code {code}";
                        _responseDto.IsSuccess = false;
                    }
                    else
                    {
                        _responseDto.Result = _mapper.Map<CouponDto>(coupon);
                    }
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
        public async Task<ResponseDto> Post([FromBody] CouponCreateDto couponDto)
        {
            try
            {
                if (couponDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "The coupon is null";
                }
                else
                {
                    Coupon coupon = _mapper.Map<Coupon>(couponDto);
                    _context.Coupons.Add(coupon);
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
        public async Task<ResponseDto> Put([FromBody] CouponDto couponDto)
        {
            try
            {
                if (couponDto == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "The coupon is null";
                }
                else
                {
                    Coupon coupon = _mapper.Map<Coupon>(couponDto);
                    _context.Coupons.Update(coupon);
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

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                Coupon? coupon = _context.Coupons.FirstOrDefault(p => p.CouponId == id);

                if (coupon == null)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = $"The coupon with id {id} doesn't exists";
                }
                else
                {
                    _context.Coupons.Remove(coupon);
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
