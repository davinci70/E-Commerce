using Microsoft.AspNetCore.Mvc;
using X.Paymob.CashIn.Models.Callback;
using X.Paymob.CashIn;

namespace e_commerce.Services.IService
{
    public interface ICashInService
    {
        Task<string> RequestCardPaymentKey(int OrderID);
        Task<bool> CashInCallback(
            [FromQuery] string hmac,
            [FromBody] CashInCallback callback,
            IPaymobCashInBroker broker
        );
    }
}
