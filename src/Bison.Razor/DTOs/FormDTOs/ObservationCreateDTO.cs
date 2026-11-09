namespace Bison.DTOs;

public class ObservationCreateDTO
{
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
}