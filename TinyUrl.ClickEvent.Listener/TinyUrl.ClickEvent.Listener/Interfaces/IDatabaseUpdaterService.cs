namespace TinyUrl.ClickEvent.Listener.Interfaces
{
    public interface IDatabaseUpdaterService
    {
        Task UpdateUrlClicksAsync(string shortUrl);

        Task ResetDailyLimitAsync();
    }
}
