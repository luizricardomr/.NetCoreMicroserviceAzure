﻿namespace Mango.Services.CouponApi.Models.DTO
{
    public class ResponseDTO
    {
        public Object? Result {  get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}