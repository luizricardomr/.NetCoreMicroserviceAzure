using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponApi.Models.DTO
{
    public class CouponDTO
    {
        [Key]
        public int? CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
