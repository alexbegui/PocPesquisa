using CensusFieldSurvey.Model.EntitesBD;

namespace CensusFieldSurvey.DataBase
{
    public class ResearchRepository(AppDbContext db) : Repository<Research>(db)
    {
    }
}
