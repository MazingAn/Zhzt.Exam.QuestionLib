using Nacos.V2;

namespace MicroService.NacosDiscover.Helper
{
    public class NacosServiceDiscover
    {
        private readonly INacosNamingService _nns;

        public NacosServiceDiscover(INacosNamingService nns)
        {
            _nns = nns;
        }

        public string GetRealUrl(string url, string serviceName, string groupoName = "DEFAULT_GROUP")
        {
            var instance = _nns.SelectOneHealthyInstance(serviceName, groupoName).Result;
            var host = $"{instance.Ip}:{instance.Port}";

            var baseUrl = instance.Metadata.TryGetValue("secure", out _)
                ? $"https://{host}"
                : $"http://{host}";

            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                return string.Empty;
            }
            return $"{baseUrl}{url}";
        }

    }
}