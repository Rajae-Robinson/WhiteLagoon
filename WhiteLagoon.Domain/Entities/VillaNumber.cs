using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Villa_Number { get; set;}
        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        [ValidateNever]
        public Villa? Villa { get; set; }
        public string? SpecialDetails { get; set; }
    }
}
