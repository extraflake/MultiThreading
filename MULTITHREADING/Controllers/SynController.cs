using MULTITHREADING.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MULTITHREADING.Controllers
{
    public class SynController : Controller
    {
        // GET: Syn
        public ActionResult Index()
        {
            ExecuteSync();
            return View();
        }

        public void ExecuteSync()
        {
            var watch = Stopwatch.StartNew();

            RunDownloadSync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            ViewBag.Stopwatch += $"Total execution time: { elapsedMs }";
        }

        public void RunDownloadSync()
        {
            List<string> websites = PrepData();

            foreach (var item in websites)
            {
                WebsiteDataModel websiteDataModel = DownloadWebsite(item);
                ReportWebsiteInfo(websiteDataModel);
            }
        }

        public List<string> PrepData()
        {
            List<string> output = new List<string>();

            ViewBag.Stopwatch = "";

            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");

            return output;
        }

        public WebsiteDataModel DownloadWebsite(string websiteUrl)
        {
            WebsiteDataModel websiteDataModel = new WebsiteDataModel();
            WebClient webClient = new WebClient();

            websiteDataModel.WebsiteUrl = websiteUrl;
            websiteDataModel.WebsiteData = webClient.DownloadString(websiteUrl);

            return websiteDataModel;
        }

        public void ReportWebsiteInfo(WebsiteDataModel websiteDataModel)
        {
            ViewBag.Stopwatch += $"{ websiteDataModel.WebsiteUrl } downloaded: { websiteDataModel.WebsiteData.Length } characters long.{ Environment.NewLine }";
        }
    }
}