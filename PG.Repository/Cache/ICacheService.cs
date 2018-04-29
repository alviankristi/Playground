namespace PG.Repository.Cache
{
    public interface ICacheService
    {
        void Add(string key, string value);
        string Get(string key);
        void Delete(string key);
    }
}
