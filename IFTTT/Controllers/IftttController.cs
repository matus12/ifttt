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
        const string KEY = "cDxBDvAD_g-2azRWdnKqlqBcRptNxeL2l3LKw8R2TJcpcyW7RSNZaPmRx-o_9R7k";

        // GET: api/Ifttt
        [Route("ifttt/v1/status")]
        public async Task<IHttpActionResult> GetAsync()
        {
            string key = "";
            IEnumerable<string> values;
            if (Request.Headers.TryGetValues("IFTTT-Channel-Key", out values))
            {
                key = values.FirstOrDefault();
            }

            if (key.Equals(KEY))
            {
                return await Task.FromResult(StatusCode(HttpStatusCode.OK));
            }

            return await Task.FromResult(StatusCode(HttpStatusCode.ServiceUnavailable));
        }

        // GET: api/Ifttt/5
        public string Get(int id)
        {
            IEnumerable<string> values;
            Request.Headers.TryGetValues("IFTTT-Channel-Key", out values);
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
