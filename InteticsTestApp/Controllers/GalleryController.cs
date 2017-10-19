using InteticsTestApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using InteticsTestApp.CustomClasses;

namespace InteticsTestApp.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        public ActionResult GalleryViewer ()
        {
            if (User.Identity.IsAuthenticated)
            {
              
                return View();
            }
           else
            {
                return RedirectToAction("Login", "Account");

            }
          
        }

        [HttpGet]
        public JsonResult GetAllMyGalleryByTag(string tagName)
        {
            return Json(InteticsTestApp.CustomClasses.Content.selectMetaData(User.Identity.Name,tagName,true), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetAllMyGallery()
        {

            List<Imagedata> timelist = InteticsTestApp.CustomClasses.Content.selectMetaData(User.Identity.Name, "", false);
            List<Imagedata> resultlist=new List<Imagedata>();
            Imagedata imd = new Imagedata(); ;
            bool check;
            foreach (var s in timelist)
            {
                check = false;
                if(resultlist.Count()==0)
                {
                    resultlist.Add(s);
                    check = true;

                }

                foreach(var n in resultlist)
                {
                  if (s.filename != n.filename && check!=true)
                    {
                        imd = s;
                       
                    }
                  else
                    {
                        check = true;
                    }

                }
                if(check!=true && imd.name!=null)
                {
                    
                    resultlist.Add(imd);
                }

            }

            return Json(resultlist, JsonRequestBehavior.AllowGet);


        }


        [HttpGet]
        public JsonResult SearchTags(string tagName)
        {


            return Json(InteticsTestApp.CustomClasses.Content.selectSearchTags(tagName), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFiles(string name, string tags, string description)
        {
            string[] tagArr = tags.Split(',').Select(x => x).ToArray();
            if (Request.Files.Count > 0 &&  name!="")
            {
               
                try
                {
                    string fileName = String.Format(name + "_{0}_{1}.jpg",
                       DateTime.Now.ToString("yyyyMMddHHmmssfff"), Guid.NewGuid());
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        bool exists = System.IO.Directory.Exists(Server.MapPath("~/image/"+ User.Identity.Name));
                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/image/" + User.Identity.Name));

                          
                        fname = System.IO.Path.Combine(Server.MapPath("~/image/"+ User.Identity.Name), fileName);
                     
                       if( InteticsTestApp.CustomClasses.Content.insertMetaData(name, fileName, tagArr, description, User.Identity.Name))
                        file.SaveAs(fname);
                  

                    }
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected or filename is empty.");
            }
        }

    }
}