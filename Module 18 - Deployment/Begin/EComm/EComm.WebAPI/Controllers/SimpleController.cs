using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EComm.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class SimpleController : Controller
    {
        private IConfiguration Configuration { get; }
        private DataContext ECommDataContext { get; }
        public SimpleController(IConfiguration configuration, DataContext dataContext)
        {
            Configuration = configuration;
            ECommDataContext = dataContext;

            Console.WriteLine("********* SimpleController *******************");
            Console.WriteLine($"ConnectionStrings:DefaultConnection " +
                $"{(Configuration["ConnectionStrings:DefaultConnection"])}");
        }

        [HttpGet]
        public string Get()
        {
            return $"Number of Products; {ECommDataContext.Products.Count()}";
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return String.Format("id = {0}", id);
        }
    }
}