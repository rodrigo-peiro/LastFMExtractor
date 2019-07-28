using LastFMExtractor.Domain.Models;
using System;

namespace LastFMExtractor.Application.TransformService
{
    public interface ITransformService
    {
        Root TransformJsonToObject(string json);

        DateTime TransformUnixDateTimeToRegularDateTime(long unixTimestamp);
    }
}
