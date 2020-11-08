using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("shows")]
    public class ShowController : Controller
    {
        private ShowService showService;

        public ShowController(ShowService showService)
        {
            this.showService = showService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(showService.GetShowLabel());
        }
    }
}
