﻿using ECommerceApi.Models;

namespace ECommerceApi.Dtos
{
    public class PaymentGetDto
    {
        public int Id { get; set; }
        public string ProductPay { get; set; }
        public Guid? UserId { get; set; }
        public string PaymentGateway { get; set; }
        public string PaymentCost { get; set; }
        public string PaymentDate { get; set; }
   
    }
}
