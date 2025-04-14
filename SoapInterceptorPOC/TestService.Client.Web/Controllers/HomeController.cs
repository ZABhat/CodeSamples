using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestService.Client.Web.Interceptors;
using TestService.Client.Web.TestWCFService;

namespace TestService.Client.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var client = new TestServiceClient();
            client.Endpoint.EndpointBehaviors.Add(new CustomEndpointBehavior());

            var result = client.GetData(4201);
            Console.WriteLine(result);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}