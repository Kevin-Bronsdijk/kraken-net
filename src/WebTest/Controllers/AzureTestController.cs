using System.Web.Mvc;

namespace WebTest.Controllers
{
    public class AzureTestController : BaseController
    {
        public ActionResult CanUploadImage()
        {
            string imagePath = Request.MapPath("~/Images/Kraken.png");

            return ReturnResults(TestLogic.AzureTests.CanUploadImage(imagePath));
        }
    }
}