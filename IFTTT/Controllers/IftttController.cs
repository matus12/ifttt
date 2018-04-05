using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IFTTT.Controllers
{
    public class IftttController : ApiController
    {
        // GET: api/Ifttt
        [Route("ifttt/v1/status")]
        public async Task<IHttpActionResult> GetAsync()
            => await Task.FromResult(StatusCode(HttpStatusCode.OK));

        // GET: api/Ifttt/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Ifttt
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Ifttt/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Ifttt/5
        public void Delete(int id)
        {
        }

        public async Task GetTask()
            => await Task.CompletedTask;
    }
}
