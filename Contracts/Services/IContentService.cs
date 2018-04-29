using System.Collections.Generic;
using Contracts.Models;

namespace Contracts.Services
{
    public interface IContentService
    {
        string ProjectId { get; set; }
        Dictionary<string, Dictionary<string, TriggerFields>> TriggerData { get; set; }
    }
}
