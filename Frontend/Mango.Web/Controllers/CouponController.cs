using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDTO>? list = new();

            ResponseDTO? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
                list = JsonConvert.DeserializeObject<List<CouponDTO>>(Convert.ToString(response.Result));
            else
                TempData["error"] = response?.Message;

            return View(list);
        }
        public async Task<IActionResult> CouponCreate(int couponId)
        {
            if(couponId > 0)
            {
                ResponseDTO? response = await GetCouponDTOById((int)couponId);

                if (response != null && response.IsSuccess)
                {
                    CouponDTO model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
                    return View(model);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDTO model)
        {
            ResponseDTO? response = null;
            string message = "";
            if (ModelState.IsValid)
            {
                if(model.CouponId > 0)
                {
                    response = await _couponService.UpdateCouponAsync(model);
                    message = "Coupon updated successfully";
                }
                else
                {
                    response = await _couponService.CreateCouponAsync(model);
                    message = "Coupon created successfully";
                }                    

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = message;
                    return RedirectToAction(nameof(CouponIndex));                    
                }
                else
                    TempData["error"] = response?.Message;
            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDTO? response = await GetCouponDTOById(couponId);

            if (response != null && response.IsSuccess)
            {
                CouponDTO model = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));                
                return View(model);
            }
            else
                TempData["error"] = response?.Message;

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDTO model)
        {
            ResponseDTO? response = await _couponService.DeleteCouponAsync(model.CouponId.Value);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
                TempData["error"] = response?.Message;
            return View(model);
        }

        private async Task<ResponseDTO> GetCouponDTOById(int couponId)
        {
            return await _couponService.GetCouponByIdAsync(couponId);
        }
    }
}
