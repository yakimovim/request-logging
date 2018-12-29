using System.Collections.Generic;
using log4net;
using Microsoft.AspNetCore.Mvc;
using RequestLogging.Logging;

namespace RequestLogging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private ILog _logger;

        private ILog Logger
        {
            get => _logger ?? (_logger = LogSupport.GetLogger(typeof(ValuesController)));
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            Logger.Info("Executing Get all");

            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            Logger.Info($"Executing Get {id}");

            return "value";
        }
    }
}
