namespace TinyUrl.ClickEvent.Listener.Interfaces
{
    public interface IUrlMappingService
    {
        Task UpdateUrlClicksAsync(string shortUrl);
    }
}
