using System.Web;

namespace LastFMExtractor.Application.UrlBuilderService
{
    public class UrlBuilderService : IUrlBuilderService
    {
        private const string method = "user.getrecenttracks";
        private const string user = "ElRoxo";
        private const string apiKey = "e38cc7822bd7476fe4083e36ee69748e";
        private const string format = "json";

        public string BuildUrlNoRecordsInDb()
        {
            return BuildUrl();
        }

        //public string BuildUrlAddLatestRecordTimestamp(string latestRecordTimestamp, string page = "1")
        //{
        //    var url = BuildUrl();
        //    var queryStringKeyValuePairs = HttpUtility.ParseQueryString(url);
        //    queryStringKeyValuePairs.Add("from", latestRecordTimestamp);
        //    queryStringKeyValuePairs.Add("page", page);

        //    return queryStringKeyValuePairs.ToString();
        //}

        //public string BuildUrlAddPage(string latestRecordTimestamp, string page)
        //{
        //    var url = BuildUrl();
        //    var queryStringKeyValuePairs = System.Web.HttpUtility.ParseQueryString(url);
        //    queryStringKeyValuePairs.Add("from", latestRecordTimestamp);
        //    queryStringKeyValuePairs.Add("page", page);

        //    return queryStringKeyValuePairs.ToString();
        //}

        public string BuildUrl(string latestRecordTimestamp = "", string page = "1")
        {
            var queryStringKeyValuePairs = HttpUtility.ParseQueryString(string.Empty);
            queryStringKeyValuePairs.Add("method", method);
            queryStringKeyValuePairs.Add("user", user);
            queryStringKeyValuePairs.Add("api_key", apiKey);            
            queryStringKeyValuePairs.Add("format", format);
            queryStringKeyValuePairs.Add("page", page);

            if (!string.IsNullOrEmpty(latestRecordTimestamp))
            {
                queryStringKeyValuePairs.Add("from", latestRecordTimestamp);
            }

            return "?" + queryStringKeyValuePairs.ToString();
        }        
    }
}
