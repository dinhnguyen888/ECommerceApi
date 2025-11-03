using System.Collections.Generic;

namespace ECommerce.TransactionService.Application.Events
{
    // Event tra ve ket qua check product tu CommerceService
    public class ProductValidationResponseEvent
    {
        public string RequestId { get; set; } // ID de match request
        public Dictionary<string, bool> ProductValidationResults { get; set; } // ProductId -> IsValid
        public string? ErrorMessage { get; set; }
    }
}
