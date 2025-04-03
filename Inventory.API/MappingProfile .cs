using AutoMapper;
using CensusFieldSurvey.Model.Common.Response;
using CensusFieldSurvey.Model.EntitesBD;

namespace CensusFieldSurvey.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Research, ResearchResponse>();
        }
    }
}
