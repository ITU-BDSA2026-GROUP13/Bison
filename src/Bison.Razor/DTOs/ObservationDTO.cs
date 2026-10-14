namespace Bison.DTOs;


public record ObservationDTO
    (
        long ObservationId,
        long AuthorId,
        string dwc_TaxonId,
        string AuthorName, 
        string TaxonName,
        string Text,
        string TimeStamp
    ); 