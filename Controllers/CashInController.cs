using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using X.Paymob.CashIn.Models.Callback;
using X.Paymob.CashIn;
using System.Text.Json.Serialization;
using e_commerce.Services.Service;
using e_commerce.Services.IService;

namespace e_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashInController : ControllerBase
    {
        private readonly ICashInService _cashInService;

        public CashInController(ICashInService cashInService)
        {
            _cashInService = cashInService;
        }

        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };

        [HttpPost("Request-Payment")]
        public async Task<IActionResult> RequestPayment(int OrderID)
        {
            try
            {
                var iframeSrc = await _cashInService.RequestCardPaymentKey(OrderID);
                return Ok(iframeSrc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("cashin-callback")]
        public async Task<IActionResult> CashInCallback(
            [FromQuery] string hmac,
            [FromBody] CashInCallback callback,
            IPaymobCashInBroker broker
        )
        {
            try
            {
                var IsValid = await _cashInService.CashInCallback(hmac, callback, broker);

                if (IsValid)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
