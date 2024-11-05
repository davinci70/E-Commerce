using e_commerce.Data;
using e_commerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Callback;
using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;

namespace e_commerce.Services.Service
{
    public class CashInService : ICashInService
    {
        private readonly IPaymobCashInBroker _broker;
        private readonly AppDbContext _Context;
        public CashInService(IPaymobCashInBroker broker, AppDbContext Context)
        {
            _broker = broker;
            _Context = Context;
        }

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        public async Task<string> RequestCardPaymentKey(int OrderID)
        {
            var Order = await _Context.Orders.FindAsync(OrderID);

            if (Order == null)
            {
                throw new Exception($"Order with ID {OrderID} is not found.");
            }

            var Customer = await _Context.Customers.FirstOrDefaultAsync(x => x.Id == Order.CustomerID);

            if (Customer == null)
            {
                throw new Exception($"Customer is not found.");
            }

            // Create order.
            var amountCents = (int)(Order.TotalPrice * 100); // LE
            var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCents);
            var orderResponse = await _broker.CreateOrderAsync(orderRequest);

            // Request card payment key.
            var billingData = new CashInBillingData(
                firstName: Customer.FirstName,
                lastName: Customer.LastName,
                phoneNumber: Customer.PhoneNumber,
                email: Customer.Email);

            var paymentKeyRequest = new CashInPaymentKeyRequest(
                integrationId: 4868912, 
                orderId: orderResponse.Id,
                billingData: billingData,
                amountCents: amountCents);

            var paymentKeyResponse = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);

            // Create iframe src.
            return _broker.CreateIframeSrc(iframeId: "879064", token: paymentKeyResponse.PaymentKey);
        }

        public async Task<bool> CashInCallback(
            [FromQuery] string hmac,
            [FromBody] CashInCallback callback,
            IPaymobCashInBroker broker
        )
        {
            if (callback.Type is null || callback.Obj is null)
            {
                throw new InvalidOperationException("Unexpected transaction callback.");
            }

            var content = ((JsonElement)callback.Obj).GetRawText();

            switch (callback.Type.ToUpperInvariant())
            {
                case CashInCallbackTypes.Transaction:
                    {
                        var transaction = JsonSerializer.Deserialize<CashInCallbackTransaction>(content, SerializerOptions)!;
                        var valid = broker.Validate(transaction, hmac);

                        if (!valid)
                        {
                            return false;
                        }

                        return true;
                    }
                default:
                    throw new InvalidOperationException($"Unexpected {nameof(CashInCallbackTypes)} = {callback.Type}");
            }
        }
    }
}
