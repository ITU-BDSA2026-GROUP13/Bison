namespace Bison.DTOs;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class ObservationCreateDTO
{
    [Required(ErrorMessage = "Choose a taxon")]
    public string TaxonId { get; set; } = "";

    [Required(ErrorMessage = "Write a message")]
    public string ObservationText { get; set; } = "";
    
    public IFormFile? Image { get; set; }

    [BindNever] public IEnumerable<SelectListItem> TaxonOptions { get; set; } = new List<SelectListItem>();

    /*
   public record ObservationDTO(
       long ObservationId,
       string AuthorId,
       string dwc_TaxonId,
       string AuthorName,
       string TaxonName,
       string Text,
       string TimeStamp,
       IFormFile? mageUrl

   );
   */
}