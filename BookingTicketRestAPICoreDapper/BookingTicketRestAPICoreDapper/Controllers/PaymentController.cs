using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTicketRestAPICoreDapper.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoMo;
using Newtonsoft.Json.Linq;

namespace BookingTicketRestAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost("WebPaymentRequest")]
        public async Task<IActionResult> WebPaymentRequest(string orderInfo, string amount)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = configMomo.partnerCode;
            string accessKey = configMomo.accessKey;
            string serectkey = configMomo.serectkey;
            string returnUrl = "https://momo.vn"; 
            string notifyurl = "https://momo.vn";

            string orderid = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            log.Debug("rawHash = " + rawHash);

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            log.Debug("Signature = " + signature);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
            log.Debug("Json request to MoMo: " + message.ToString());
            string responseFromMomo =  PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            log.Debug("Return from MoMo: " + jmessage.ToString());
            //DialogResult result = MessageBox.Show(responseFromMomo, "Open in browser", MessageBoxButtons.OKCancel);
            //if (result == DialogResult.OK)
            //{
            //    //yes...
             //return Ok(System.Diagnostics.Process.Start(jmessage.GetValue("payUrl").ToString()));
            //}
            //else if (result == DialogResult.Cancel)
            //{
            //    //no...
            //}
            return Ok(jmessage.GetValue("payUrl").ToString());
        }
    }
}