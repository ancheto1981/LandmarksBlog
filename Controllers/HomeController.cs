using LandmarksBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Data;

namespace LandmarksBlog.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();

            var posts = db.Posts.Include(p => p.Author)
                .OrderByDescending(p => p.Date)
                .Take(4);

            return View(posts.ToList());
        }

        public ActionResult Pictures(HttpPostedFileBase file)
        {
            List<string> items = (List<string>)this.Session["items"] ?? new List<string>();

            if (file != null && file.ContentLength > 0)
                try
                {
                    string imagePath = Path.Combine(Server.MapPath("images/"),
                                       Path.GetFileName(file.FileName));
                    file.SaveAs(imagePath);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View(items);
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult AddItem(string newItem)
        {
            List<string> items = (List<string>)this.Session["items"] ?? new List<string>();
            items.Add(newItem);
            this.Session["items"] = items;

            return RedirectToAction("Pictures");
        }

        [Authorize(Roles ="Admin")]
       
        public ActionResult RemoveItem(int index)
        {
            List<string> items = (List<string>)this.Session["items"];

            if (items != null && index < items.Count)
            {
                items.RemoveAt(index);this.Session["items"] = items;
            }
            return this.RedirectToAction("Pictures");
        }
    }
}
  