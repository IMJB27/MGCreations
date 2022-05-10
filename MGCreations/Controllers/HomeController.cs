using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MGCreations.Models;
using System.Net;

namespace MGCreations.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost] /*https://stackoverflow.com/questions/30323183/contact-us-form-asp-net-mvc */
        public async Task<ActionResult> ContactUs(String name, String email, String subject, String message)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                    var mailMessage = new MailMessage();
                    mailMessage.To.Add(new MailAddress("jaybharakhda@gmail.com"));  // replace with valid value 
                    mailMessage.From = new MailAddress(email);  // replace with valid value
                    mailMessage.Subject = subject;
                    mailMessage.Body = string.Format(body, name, email, message);
                    mailMessage.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "jaybharakhda@gmail.com",  // replace with valid value
                            Password = "@Jmb0756@"  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";//address webmail
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mailMessage);
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    
                    return View();
                }
            }
            return View();

        }
    }
}