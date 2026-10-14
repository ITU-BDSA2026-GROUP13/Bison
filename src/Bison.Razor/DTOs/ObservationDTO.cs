namespace Bison.DTOs;


public record ObservationDTO
    (
        int ObservationId,
        int AuthorId,
        string dwc_TaxonId,
        string AuthorName, 
        string TaxonName,
        string Text,
        string TimeStamp
    ); 