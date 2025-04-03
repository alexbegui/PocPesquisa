namespace CensusFieldSurvey.API
{
    public class ApiEndpointsOptions
    {
        public string? IpProducao { get; set; }
        public string? IpHomologacao { get; set; }
        public string? EndPointBaseAPI { get; set; }
        public string? EndPointToken { get; set; }
        public string? AmbienteApiEndpoints { get; set; }


    }


    //public class ConfigService
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly DienConfig _dienConfig;

    //    public ConfigService(IConfiguration configuration)
    //    {
    //        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    //        _dienConfig = _configuration.GetSection("DienSettings").Get<DienConfig>() ?? throw new ArgumentNullException(nameof(_dienConfig));
    //    }

    //    public string? GetIpProducao()
    //    {
    //        return _dienConfig.IpProducao;
    //    }

    //    public string? GetIpHomologacao()
    //    {
    //        return _dienConfig.IpHomologacao;
    //    }

    //    public string? GetEndPointBaseAPI()
    //    {
    //        return _dienConfig.EndPointBaseAPI;
    //    }

    //    public string? GetEndPointToken()
    //    {
    //        return _dienConfig.EndPointToken;
    //    }
    //}

}


