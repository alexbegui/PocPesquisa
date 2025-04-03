using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CensusFieldSurvey.Model.EntitesBD
{
    [Table("tbResearch")]
    public class Research
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("idResearch")]
        public int IdResearch { get; set; }

        [Required(ErrorMessage = "The form field is required.")]
        [Column("form", TypeName = "jsonb")]
        public string? Form { get; set; }
    }
}
