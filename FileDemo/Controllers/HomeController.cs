using FileDemo.Models;
using FileDemo.SpecialClass;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace FileDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly AppDBContext _appContext;

        public HomeController(ILogger<HomeController> logger,
              IWebHostEnvironment hostingEnvironment,
              AppDBContext appContext)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _appContext = appContext;
        }

        public IActionResult Index()
        {
           var userId = HttpContext.Session.GetInt32("UserId");
            AttachmentViewModel model = new AttachmentViewModel();
            model.attachments = _appContext.attachments.Where(attachment => attachment.SId == userId).ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Index(AttachmentViewModel model)
        {
            if (model.attachment != null)
            {

                var uniqueFileName = SPClass.CreateUniqueFileExtension(model.attachment.FileName);
                var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "attachment");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.attachment.CopyTo(new FileStream(filePath, FileMode.Create));


                Attachment attachment = new Attachment();
                attachment.FileName = uniqueFileName;
                attachment.Description = model.Description;
                attachment.attachment = SPClass.GetByteArrayFromImage(model.attachment);
                attachment.SId = (int)HttpContext.Session.GetInt32("UserId");

                _appContext.attachments.Add(attachment);
                _appContext.SaveChanges();
            }
            return RedirectToAction("index");
        }


        [HttpGet]
        public ActionResult GetAttachment(int ID)
        {
            byte[] fileContent;
            string fileName = string.Empty;
            Attachment attachment = new Attachment();
            attachment = _appContext.attachments.Select(m => m).Where(m=> m.id == ID).FirstOrDefault();

            string contentType = SPClass.GetContenttype(attachment.FileName);
            fileContent = (byte[])attachment.attachment;
            return new FileContentResult(fileContent, contentType);
        }

        [HttpGet]
        public PhysicalFileResult GetPhysicalFileResultDemo(string filename)
        {
            string path = "/wwwroot/attachment/" + filename;
            string contentType = SPClass.GetContenttype(filename);
            return new PhysicalFileResult(_hostingEnvironment.ContentRootPath
                + path, contentType);
        }

        // Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Student login)
        {
           var user = _appContext.students.Where(u => u.Name.Equals(login.Name) && u.Password.Equals(login.Password)).SingleOrDefault();

            if (user != null)
            {
                var userId = user.Id;
                HttpContext.Session.SetInt32("UserId", userId);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            return RedirectToAction("Login", "Home");
        }

        // Delete 

        [HttpGet]
        public ActionResult Delete(int ID)
        {
            
            Attachment attachment = new Attachment();
            _appContext.attachments.Remove(_appContext.attachments.Where(x => x.id == ID).FirstOrDefault());
            _appContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DeleteAll()
        {

            Attachment attachment = new Attachment();
            var userId = HttpContext.Session.GetInt32("UserId");
            _appContext.attachments.RemoveRange(_appContext.attachments.Where(x => x.SId == userId).ToList());
            _appContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}