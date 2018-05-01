using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using Contracts.Models;
using Contracts.Services;
using Contracts.Wrappers;
using KenticoCloud.Delivery;

namespace IFTTT.Controllers
{
    public class IftttController : ApiController
    {
        private const string Key = "cDxBDvAD_g-2azRWdnKqlqBcRptNxeL2l3LKw8R2TJcpcyW7RSNZaPmRx-o_9R7k";
        private readonly IContentService _contentService;
        private readonly HttpClient _httpClient;


        public IftttController(
            IContentService contentService,
            IHttpClientWrapper httpClientWrapper)
        {
            _contentService = contentService;
            _httpClient = httpClientWrapper.HttpClient;
        }

        // GET: api/Ifttt
        [Route("ifttt/v1/status")]
        public async Task<IHttpActionResult> GetAsync() =>
            IsKeyValid()
                ? await Task.FromResult(StatusCode(HttpStatusCode.OK))
                : await Task.FromResult(StatusCode(HttpStatusCode.Unauthorized));


        [Route("ifttt/v1/triggers/content")]
        public async Task<IHttpActionResult> PostContentAsync([FromBody] RequestObject requestObject)
        {
            if (!IsKeyValid())
            {
                return Content(HttpStatusCode.Unauthorized, await Task.FromResult(new ContentError()));
            }

            // var contentToReturn = _contentService.TriggerData.Values.First();
            //var projectId = _contentService.ProjectId;
            //_contentService.TriggerData.Remove(projectId);

            if (requestObject == null || requestObject.limit == -1)
            {
                return Ok(await Task.FromResult(new Content("f5dd2c63-2de7-48cc-8ff6-129e939491c0", "wtf")));
                //return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
            }

            if (_contentService.TriggerData.TryGetValue(requestObject.TriggerFields.Project_id, out var triggerData))
            {
                triggerData.TryGetValue(requestObject.Trigger_identity, out var triggerFields);

                _contentService.TriggerData[requestObject.TriggerFields.Project_id].Remove(requestObject.Trigger_identity);

                return Ok(await Task.FromResult(new Content(triggerFields)));
            }

            var map = new Dictionary<string, TriggerFields>
            {
                {requestObject.Trigger_identity, requestObject.TriggerFields}
            };

            _contentService.TriggerData.Add(requestObject.TriggerFields.Project_id, map);

            //_contentService.TriggerData.Add(_contentService.ProjectId, requestObject.Trigger_identity);

            // await PostContent(_contentService.TriggerData.Values.First());

            return Ok(await Task.FromResult(new Content("f5dd2c63-2de7-48cc-8ff6-129e939491c0", "wtf")));
            //return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }

        [HttpPost]
        [Route("ifttt/v1/test/setup")]
        public async Task<IHttpActionResult> Post([FromBody] string value)
        {
            if (IsKeyValid())
            {
                return Ok(await Task.FromResult(new Response()));
            }

            return await Task.FromResult(StatusCode(HttpStatusCode.Unauthorized));
        }

        [HttpPost]
        [Route("ifttt/v1/hook")]
        public async Task<IHttpActionResult> Post([FromBody] Webhook webhook)
        {
            var deliveryClient = new DeliveryClient("f5dd2c63-2de7-48cc-8ff6-129e939491c0");
            var response = await deliveryClient.GetItemAsync(webhook.Data.Items.First().Codename);
            var elements = response.Item.Elements;
            var triggerIdentity = "";

            _contentService.ProjectId = webhook.Message.ProjectId.ToString();

            await PostRealtime(webhook.Message.ProjectId.ToString());

            var requestMessage =
                new HttpRequestMessage(
                    HttpMethod.Put,
                    "https://webhook.site/15851e96-6f08-4d9d-9e9b-11ff19e057d6"
                );
            var children = elements.Children();
            var str = "";
            var list = new List<string>();
            foreach (var result in children)
            {
                var type = result.First.type.ToString();
                var value = result.First.value.ToString();
                if (type.Equals("multiple_choice"))
                {
                    continue;
                }

                if (type.Equals("rich_text"))
                {
                    var strippedText = StripHtml(value);
                    str += strippedText;
                    list.Add(strippedText);
                }
                else
                {
                    str += value + " ";
                    list.Add(value);
                }
            }

            var triggerFields = new TriggerFields(_contentService.ProjectId);

            for (var i = 1; i < 11; i++)
            {
                var info = triggerFields.GetType().GetField("Value" + i);
                info.SetValue(triggerFields, list[i]);
            }

            /*TODO: vyriesit aby sa id v meta generovalo nove iba pri webhooku*/
            if (_contentService.TriggerData.TryGetValue(webhook.Message.ProjectId.ToString(), out var triggerData))
            {
                foreach (var entry in triggerData)
                {
                    if (entry.Value.Content_item_codename != webhook.Data.Items[0].Codename) continue;
                    triggerIdentity = entry.Key;
                    _contentService.TriggerData[webhook.Message.ProjectId.ToString()][triggerIdentity] = triggerFields;
                }
            }

            requestMessage.Headers.Add(
                "IFTTT-Service-Key",
                Key
            );
            requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate");
            requestMessage.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());
            requestMessage.Content = new StringContent(
                "{" +
                "\"data\": " +
                "[ { \"trigger_identity\": \"" +
                triggerIdentity +
                "\" }, ] }",
                Encoding.UTF8,
                "application/json"
            );

            await _httpClient.SendAsync(requestMessage);

            return await Task.FromResult(StatusCode(HttpStatusCode.OK));
        }

        private bool IsKeyValid()
        {
            var key = "";

            if (Request.Headers.TryGetValues("IFTTT-Channel-Key", out var values))
            {
                key = values.FirstOrDefault();
            }

            return key != null && key.Equals(Key);
        }

        private async Task PostContent(string value = "")
        {
            var content = new StringContent(
                "{\"text\":\"" + _contentService.ProjectId + " " + value + "\"}",
                Encoding.UTF8,
                "application/json"
            );
            await _httpClient.PostAsync("https://webhook.site/15851e96-6f08-4d9d-9e9b-11ff19e057d6", content);
        }

        private async Task PostRealtime(string projectId)
        {
            if (_contentService.TriggerData.TryGetValue(projectId, out var triggerInfo))
            {
                var content = new StringContent(
                    "{" +
                    "\"data\": " +
                    "[ { \"trigger_identity\": \"" +
                    triggerInfo.Keys.First() + "REALTIME BITCH!" +
                    "\" }, ] }",
                    Encoding.UTF8,
                    "application/json"
                );
                content.Headers.Add(
                    "IFTTT-Service-Key",
                    "cDxBDvAD_g-2azRWdnKqlqBcRptNxeL2l3LKw8R2TJcpcyW7RSNZaPmRx-o_9R7k"
                );
                //content.Headers.Add("Accept-Encoding", "gzip, deflate");
                content.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());

                await _httpClient.PostAsync("https://webhook.site/15851e96-6f08-4d9d-9e9b-11ff19e057d6", content);
            }
        }

        private static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}