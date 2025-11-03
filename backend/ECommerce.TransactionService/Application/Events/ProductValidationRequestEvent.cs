using System.Collections.Generic;

namespace ECommerce.TransactionService.Application.Events
{
    // Event de yeu cau CommerceService check thong tin product
    public class ProductValidationRequestEvent
    {
        public string RequestId { get; set; } // ID de match response
        public List<string> ProductIds { get; set; }
    }
}
