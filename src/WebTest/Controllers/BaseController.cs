using System.Web.Mvc;

namespace WebTest.Controllers
{
    public abstract class BaseController : Controller
    {
        public ActionResult Index()
        {
            // Used for debugging
            return null;
        }

        internal ActionResult ReturnResults(bool success)
        {
            if (success)
                return new HttpStatusCodeResult(200);
            else
                return new HttpStatusCodeResult(500);
        }
    }
}