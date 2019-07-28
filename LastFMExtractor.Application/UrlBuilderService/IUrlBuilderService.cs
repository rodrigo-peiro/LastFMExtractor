namespace LastFMExtractor.Application.UrlBuilderService
{
    public interface IUrlBuilderService
    {
        string BuildUrl(string latestRecordTimestamp = "", string page = "1");
        //string BuildUrlNoRecordsInDb();
        //string BuildUrlAddLatestRecordTimestamp(string latestRecordTimestamp, string page = "1");
        //string BuildUrlAddPage(string latestRecordTimestamp, string page);
    }
}
