using System.Collections.Generic;
using Contracts.Models;

namespace Contracts.Services
{
    public interface IContentService
    {
        Dictionary<string, Dictionary<string, TriggerFields>> TriggerData { get; set; }
    }
}
