using System.ComponentModel.DataAnnotations;

namespace CensusFieldSurvey.Model.Common.Request
{
    public class ResearchRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The form field is required.")]
        public string? Form { get; set; }
    }

}
