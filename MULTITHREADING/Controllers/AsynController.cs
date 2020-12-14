using MULTITHREADING.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MULTITHREADING.Controllers
{
    public class AsynController : Controller
    {
        // GET: Asyn
        public async Task<ActionResult> Index()
        {
            await ExecuteAsync();
            return View();
        }

        public async Task ExecuteAsync()
        {
            var watch = Stopwatch.StartNew();

            await RunDownloadParallelAsync();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            ViewBag.Stopwatch += $"Total execution time: { elapsedMs }";
        }

        public async Task RunDownloadAsync()
        {
            List<string> websites = PrepData();

            foreach (var item in websites)
            {
                WebsiteDataModel websiteDataModel = await Task.Run(() => DownloadWebsite(item));
                ReportWebsiteInfo(websiteDataModel);
            }
        }

        public async Task RunDownloadParallelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            foreach (var item in websites)
            {
                tasks.Add(Task.Run(() => DownloadWebsite(item)));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var item in results)
            {
                ReportWebsiteInfo(item);
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