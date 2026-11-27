namespace Bison.DTOs;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class ProposalCreateDTO
{
    [Required(ErrorMessage = "Choose a taxon")]
    public string TaxonId { get; set; } = "";

    [BindNever] public IEnumerable<SelectListItem> TaxonOptions { get; set; } = new List<SelectListItem>(); 
    
}

