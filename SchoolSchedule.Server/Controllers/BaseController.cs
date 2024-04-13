using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SchoolSchedule.Server.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult HandleException(Exception ex)
        {
            if (ex is ArgumentNullException)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while processing the request.");
            }
            else if (ex is InvalidOperationException)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "More than one element satisfies the condition in SingleOrDefault.");
            }
            else if (ex is DbUpdateException)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while updating the database.");
            }
            else
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
