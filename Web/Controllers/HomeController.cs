using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Web.Extensions;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : ControllerBase
    {
        //[OutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            return View();
        }

        private List<RequestDataViewModel> GetData(object objectToGetData, string category)
        {
            List<RequestDataViewModel> list = new List<RequestDataViewModel>();
            var type = objectToGetData.GetType();
            var props = type.GetProperties();
            XDocument xdoc = XDocument.Parse(WebXmlCache.Xml);
            var allMembers = xdoc.Descendants("member");




            foreach (var prop in props)
            {
                var length = prop.GetIndexParameters().Length;
                if (length > 0)
                {
                    // Йордан: Това е индексатор
                    continue;
                    //public virtual string this[string key] { get; }                   
                }


                string keyInXml = "P:System.Web.HttpBrowserCapabilitiesBase." + prop.Name;
                var descriptionMember = allMembers.Where(c => c.Attribute("name")?.Value == keyInXml).FirstOrDefault();
                string summary = string.Empty;
                string returns = string.Empty;
                if (descriptionMember != null)
                {
                    foreach (var descNode in descriptionMember.Elements())
                    {
                        if (descNode.Name.LocalName.Equals("summary", StringComparison.InvariantCultureIgnoreCase))
                        {
                            summary = descNode.Value;
                            if (summary.StartsWith("When overridden in a derived class, "))
                            {
                                summary = summary.Replace("When overridden in a derived class, ", string.Empty);
                                string firstCharToUpper = summary[0].ToString().ToUpper();
                                summary = firstCharToUpper + summary.Substring(1);
                            }

                        }
                        if (descNode.Name.LocalName.Equals("returns", StringComparison.InvariantCultureIgnoreCase))
                        {
                            returns = descNode.Value;
                        }
                    }
                }

                

                    //string description = 
                    var val = prop.GetValue(objectToGetData);

                if (!(val is string) && val is IEnumerable)
                {
                    continue;
                }
                list.Add(new RequestDataViewModel() { Name = prop.Name, Value = val?.ToString()?.TrimToLength(),
                    Summary = summary, Returns = returns, Category= category
                });

                if (!(val is string) && val is IEnumerable)
                {

                    //var ienum = val as IEnumerable;
                    //IEnumerator enumerator = ienum.GetEnumerator();
                    //int i = 0;
                    //while (enumerator.MoveNext())
                    //{
                    //    var currValue = enumerator.Current;
                    //    if (currValue is DictionaryEntry)
                    //    {
                    //        DictionaryEntry de = (DictionaryEntry)currValue;
                    //        list.Add(new RequestDataViewModel()
                    //        {
                    //            Name = $"{prop.Name}.{de.Key}",
                    //            Value = de.Value?.ToString(),
                    //            Summary = string.Empty,
                    //            Returns = string.Empty
                    //        });
                    //    }
                    //    else
                    //    {
                    //        list.Add(new RequestDataViewModel()
                    //        {
                    //            Name = $"{prop.Name}.{i}",
                    //            Value = currValue?.ToString(),
                    //            Summary = string.Empty,
                    //            Returns = string.Empty
                    //        });
                    //    }
                    //    i++;
                    //}
                }


            }

            return list;

        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            List<RequestDataViewModel> list = GetData(Request.Browser, "Browser");
            list.AddRange(GetData(Request, "Request"));
            //list.AddRange(GetData(Request));
            //list.AddRange(GetData(Request.Params));
            foreach (var item in list)
            {

                item.Value = item.Value.TrimToLength();                
            }

            foreach (object item in Request.Browser.Browsers)
            {               
                list.Add(new RequestDataViewModel()
                {
                    Name = string.Empty,
                    Value = item.ToString(),
                    Category = "Browsers"
                });
            }

            foreach (DictionaryEntry item in Request.Browser.Capabilities)
            {
                
                list.Add(new RequestDataViewModel()
                {
                    Name = item.Key as string,
                    Value = item.Value as string,
                    Category = "Capabilities"
                });
            }

            foreach (string item in Request.ServerVariables)
            {
                list.Add(new RequestDataViewModel() { Name= "ServerVariables." + item, Value = Request.ServerVariables[item],
                Category = "ServerVariables"
                });
            }

            foreach (string key in Request.Form.AllKeys)
            {
                list.Add(new RequestDataViewModel() { Name = "Form." + key, Value = Request.Form[key].TrimToLength(),
                Category = "Form"
                });
            }
            foreach (string item in Request.Headers)
            {
                list.Add(new RequestDataViewModel() { Name = "Headers." + item, Value = Request.Headers[item].TrimToLength(),
                Category = "Headers"
                });
            }
            foreach (string item in Request.QueryString)
            {
                list.Add(new RequestDataViewModel() { Name = "QueryString." + item, Value = Request.QueryString[item].TrimToLength(),
                Category = "QueryString"
                });
            }
            for (int i = 0; i < Request.Cookies.Count; i++)
            {
                list.Add(new RequestDataViewModel() { Name = "Cookies." + Request.Cookies[i].Name, Value = Request.Cookies[i].Value.TrimToLength(),
                Category = "Cookies"});
            }

           

            DataSourceResult dataSourceResult = list.ToDataSourceResult(request);
            var result = Json(dataSourceResult);
            return result;
        }
    }
}