using LastFMExtractor.Domain.Models;
using Newtonsoft.Json;
using System;

namespace LastFMExtractor.Application.TransformService
{
    public class TransformService : ITransformService
    {
        public Root TransformJsonToObject(string json)
        {
            return JsonConvert.DeserializeObject<Root>(json);
        }

        public DateTime TransformUnixDateTimeToRegularDateTime(long unixTimestamp)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);

            return offset.ToLocalTime().DateTime;
        }
    }
}
