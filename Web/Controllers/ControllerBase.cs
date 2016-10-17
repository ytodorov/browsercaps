using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return base.Json(data, contentType, contentEncoding);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            behavior = JsonRequestBehavior.AllowGet;
            return base.Json(data, contentType, contentEncoding, behavior);
        }
    }
}