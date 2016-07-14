using System.Web.Mvc;

namespace WebTest.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult CanCreateClient()
        {
            return ReturnResults(TestLogic.Tests.CanCreateClient());
        }

        public ActionResult CanUploadImage()
        {
            string imagePath = Request.MapPath("~/Images/Kraken.png");

            return ReturnResults(TestLogic.Tests.CanUploadImage(imagePath));
        }
    }
}