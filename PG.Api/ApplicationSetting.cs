using System.Configuration;

namespace PG.Api
{
    public static class ApplicationSetting
    {
        public static string CacheConnection
        {
            get
            {
                return ConfigurationManager.AppSettings["CacheConnection"];
            }
        }

        public static bool EnableCache
        {
            get
            {
                var sValue = ConfigurationManager.AppSettings["EnableCache"];
                bool.TryParse(sValue, out var value);
                return value;
            }
        }
    }
}