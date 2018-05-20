using System.Collections.Generic;
using Contracts.Models;
using Contracts.Services;

namespace ContentService.Services
{
    internal class ContentService : IContentService
    {
        public Dictionary<string, Dictionary<string, TriggerFields>> TriggerData { get; set; }

        public ContentService()
        {
            TriggerData = new Dictionary<string, Dictionary<string, TriggerFields>>();
        }
    }
}
