using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FlowerAndGiftShop.Controllers
{
    public class SharedController : Controller
    {
        public void ChangeLanguage(string language, string isUserAuthorized)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language ?? "en-EN");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language ?? "en-EN");

            CultureInfo tempCulture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();

            tempCulture.NumberFormat.NumberDecimalSeparator = ".";
            tempCulture.NumberFormat.NumberGroupSeparator = ",";
            tempCulture.NumberFormat.PercentDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentUICulture = tempCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = tempCulture;

            
                Response.Redirect("../Home/Index");
            
                ///Response.Redirect(Request.UrlReferrer.ToString(), true);
        }

         public ActionResult ChangeCulture(string lang, string returnUrl)
         {
              Session["Culture"] = new CultureInfo(lang);
              return Redirect(returnUrl);
         }

        // GET: Shared
        public ActionResult Index()
        {
            return View();
        }

        //public static void SetCurrentCulture(string currentLanguage)
        //{
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLanguage ?? ConfigurationManager.AppSettings["DefaultLanguage"]);
        //    Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLanguage ?? ConfigurationManager.AppSettings["DefaultLanguage"]);

        //    CultureInfo tempCulture = (CultureInfo)CultureInfo.CurrentUICulture.Clone();

        //    tempCulture.NumberFormat.NumberDecimalSeparator = ".";
        //    tempCulture.NumberFormat.NumberGroupSeparator = ",";
        //    tempCulture.NumberFormat.PercentDecimalSeparator = ".";

        //    System.Threading.Thread.CurrentThread.CurrentUICulture = tempCulture;
        //    System.Threading.Thread.CurrentThread.CurrentCulture = tempCulture;
        //}

    }
}