using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TwitterApiClient.Core;

namespace TwitterApiClient.ApiCallback.Controllers
{
    public class CallbackController : Controller
    {
        //
        // GET: /Callback/

        public ActionResult Index()
        {
            var url = "https://www.linkedin.com/uas/oauth2/authorization?response_type=code"
                        + "&client_id=" + "751ku61912s1ly"
                        + "&state=STATE"
                        + "&redirect_uri=" + Uri.EscapeDataString("http://localhost:56712/Callback/AccessCodeCallback");
            return Redirect(url);
        }

        public ActionResult AccessCodeCallback(string code, string state)
        {
            // todo: make sure state is the same

            var client = new HttpClientService();

            var sign = "grant_type=authorization_code" +
           "&code=" + code +
           "&redirect_uri=" + Uri.EscapeDataString("http://localhost:56712/Callback/AccessCodeCallback") +
           "&client_id=" + "751ku61912s1ly" +
           "&client_secret=" + "mhN9E3rMW2bP8V2H";

            var content = new StringContent(sign);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var data = client.Post(new HttpParameters()
            {
                Content = content,
                DefaultHeaders = new Dictionary<string, string>()
                        {
                            { "Accept-Encoding", "gzip" }
                        },
                BaseUrl = "https://www.linkedin.com/",
                ResourceUrl = "uas/oauth2/accessToken"
            });


            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            return View(payLoad);
        }

    }
}
