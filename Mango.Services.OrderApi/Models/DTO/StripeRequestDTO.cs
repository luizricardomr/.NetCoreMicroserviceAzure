namespace Mango.Services.OrderApi.Models.DTO
{
	public class StripeRequestDTO
	{
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessioId { get; set; }
        public string ApprovedUrl { get; set; }
        public string CancelUrl { get; set; }

        public OrderHeaderDTO OrderHeader { get; set; }
    }
}
