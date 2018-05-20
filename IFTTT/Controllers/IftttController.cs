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

            var projectId = requestObject.TriggerFields.Project_id;

            if (_contentService.TriggerData.TryGetValue(projectId, out var triggerData))
            {
                if (triggerData.TryGetValue(requestObject.TriggerFields.Content_item_codename,
                    out var triggerFields))
                {
                    if (!triggerFields.Trigger_identity.Contains(requestObject.Trigger_identity))
                    {
                        triggerFields.Trigger_identity.Add(requestObject.Trigger_identity);
                    }

                    return Ok(await Task.FromResult(new Content(triggerFields)));
                }

                _contentService.TriggerData[projectId] = new Dictionary<string, TriggerFields>
                {
                    {
                        requestObject.TriggerFields.Content_item_codename,
                        requestObject.TriggerFields
                    }
                };
            }
            else
            {
                requestObject.TriggerFields.Trigger_identity.Add(requestObject.Trigger_identity);
                _contentService.TriggerData.Add(
                    projectId,
                    new Dictionary<string, TriggerFields>
                    {
                        {
                            requestObject.TriggerFields.Content_item_codename,
                            requestObject.TriggerFields
                        }
                    });
            }

            return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
        }

        [HttpPost]
        [Route("ifttt/v1/test/setup")]
        public async Task<IHttpActionResult> Post([FromBody] string value)
        {
            if (IsKeyValid())
            {
                return Ok(await Task.FromResult(new SetupResponse()));
            }

            return await Task.FromResult(StatusCode(HttpStatusCode.Unauthorized));
        }

        [HttpPost]
        [Route("ifttt/v1/webhook")]
        public async Task<IHttpActionResult> Post([FromBody] Webhook webhook)
        {
            var projectId = webhook.Message.ProjectId.ToString();
            var codename = webhook.Data.Items[0].Codename;

            if (webhook.Message.Operation != "publish"
                && (webhook.Message.Type != "content_item_variant" || webhook.Message.Type != "content_item"))
            {
                return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));
            }

            if (_contentService.TriggerData.TryGetValue(projectId, out var items) && !items.ContainsKey(codename))
                return await Task.FromResult(StatusCode(HttpStatusCode.NoContent));

            var deliveryClient = new DeliveryClient(projectId);
            var response = await deliveryClient.GetItemAsync(webhook.Data.Items.First().Codename);
            var elements = response.Item.Elements;
            var children = elements.Children();
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
                    list.Add(strippedText);
                }
                else if (type.Equals("number"))
                {
                    list.Add(value.ToString());
                }
                else
                {
                    list.Add(value);
                }
            }

            var triggerFields = new TriggerFields(projectId)
            {
                Content_item_codename = codename,
                Project_id = projectId,
            };

            for (var i = 1; i < list.Count + 1; i++)
            {
                var info = triggerFields.GetType().GetProperty("Value" + i);
                if (info != null) info.SetValue(triggerFields, list[i - 1]);
            }

            if (_contentService.TriggerData.TryGetValue(projectId, out var contentData))
            {
                if (contentData.TryGetValue(codename, out var triggerData))
                {
                    triggerFields.Trigger_identity = triggerData.Trigger_identity;
                    _contentService.TriggerData[projectId][codename] = triggerFields;

                    foreach (var entry in triggerData.Trigger_identity)
                    {
                        await PostRealtime(entry);
                    }
                }
                else
                {
                    _contentService.TriggerData[projectId].Add(codename, triggerFields);
                }
            }
            else
            {
                var triggerData = new Dictionary<string, TriggerFields>()
                {
                    {codename, triggerFields}
                };
                _contentService.TriggerData.Add(projectId, triggerData);
            }

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

        private async Task PostRealtime(string triggerIdentity)
        {
            var content = new StringContent(
                "{" +
                "\"data\": " +
                "[ { \"trigger_identity\": \"" +
                triggerIdentity +
                "\" } ] }",
                Encoding.UTF8,
                "application/json"
            );
            content.Headers.Add(
                "IFTTT-Service-Key",
                Key
            );
            content.Headers.Add("X-Request-ID", Guid.NewGuid().ToString());

            await _httpClient.PostAsync("https://realtime.ifttt.com/v1/notifications", content);
        }

        private static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}