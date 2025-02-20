using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using Net.payOS;
using Net.payOS.Types;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Transactions;

public class PayosService
{
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;
    private readonly IConfiguration _configuration;
    private string clientId;
    private string apiKey;
    private string checksumKey;
    private string returnUrl;
    private string cancelUrl;
    private PayOS _payOS;
    private readonly AppDbContext _dbContext;
    public PayosService(IMapper mapper, IPaymentService paymentService, IConfiguration configuration, AppDbContext appDbContext  )
    {
        _configuration = configuration;
        _mapper = mapper;
        _paymentService = paymentService;
        clientId = _configuration["PayOS:CLIENT_ID"] ?? throw new ArgumentNullException("Client ID is missing");
        apiKey = _configuration["PayOS:API_KEY"] ?? throw new ArgumentNullException("API Key is missing");
        checksumKey = _configuration["PayOS:CHECKSUM_KEY"] ?? throw new ArgumentNullException("Checksum Key is missing");
        cancelUrl = _configuration["PayOS:CANCEL_URL"] ?? throw new ArgumentNullException("Cancel URL is missing");
        returnUrl = _configuration["PayOS:RETURN_URL"] ?? throw new ArgumentNullException("Return URL is missing");

        PayOS payos = new PayOS(clientId, apiKey, checksumKey);
        _payOS = payos;
        _dbContext = appDbContext;
    }

    public async Task<string> CreatePaymentLink(PaymentPostDto body)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            // Create payment in database
            var payment = _mapper.Map<PaymentPostDto>(body);
            string paymentId = await _paymentService.CreatePaymentAsync(payment);

            // check payment ID is valid
            if (!int.TryParse(paymentId, out int orderCode))
            {
                throw new InvalidOperationException($"Invalid payment ID: {paymentId}");
            }

            int price = (int)body.ProductPrice;
            ItemData item = new ItemData(body.ProductPay, 1, price);
            List<ItemData> items = new List<ItemData> { item };
            PaymentData paymentData = new PaymentData(orderCode, price, body.ProductPay, items, cancelUrl, returnUrl);

            //create payment link
            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            //commit transaction
            await transaction.CommitAsync();

            return createPayment.checkoutUrl;
        }
        catch (FormatException ex)
        {
            await transaction.RollbackAsync();
            throw new InvalidOperationException("Payment ID is not in a valid format.", ex);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw; 
        }
    }
    public async Task<PaymentLinkInformation> GetOrderAsync(int orderId)
    {
        return await _payOS.getPaymentLinkInformation(orderId);
    }

    public async Task<bool> ReceiveWebhook(WebhookType webhook)
    {

        //using transaction to ensure that the payment is updated and email is sent
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var payment = await _paymentService.ChangePaymentStatusAndGetPaymentInfo(true, webhook.data.orderCode);
            if (payment == null)
            {
                throw new ArgumentNullException("Payment not found");
            }

            await _paymentService.SendEmailUsingPaymentInfo(payment);
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<PaymentLinkInformation> CancelOrderAsync(int orderId)
    {
        return await _payOS.cancelPaymentLink(orderId);
    }

    public async Task ConfirmWebhookAsync(string webhookUrl)
    {
        await _payOS.confirmWebhook(webhookUrl);
    }


    
}


