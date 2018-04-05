using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            return await Task.FromResult(StatusCode(HttpStatusCode.Unauthorized));
        }

        [Route("ifttt/v1/triggers/content")]
        public async Task<IHttpActionResult> PostContentAsync()
        {
            return Ok(await Task.FromResult(new Content()));
        }

        // POST: api/Ifttt
        [HttpPost]
        [Route("ifttt/v1/test/setup")]
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            string key = "";
            IEnumerable<string> values;
            if (Request.Headers.TryGetValues("IFTTT-Channel-Key", out values))
            {
                key = values.FirstOrDefault();
            }

            if (key.Equals(KEY))
            {
                return Ok(await Task.FromResult(new Response()));
            }

            return await Task.FromResult(StatusCode(HttpStatusCode.Unauthorized));
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
