using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWT.Controllers
{
    [Authorize]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetId(int id)
        {
            var customerAlert = "hi";
            return Ok(customerAlert);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var customersAlert = new string[] { "customer-1", "customer-2", "customer-3" };
            return Ok(customersAlert);
        }
    }
}
