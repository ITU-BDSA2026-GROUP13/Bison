namespace Bison.DTOs;


public record ProposalDTO
    (
        long PostId,
        string AuthorId,
        string dwc_TaxonId, 
        string TaxonName,
        string AuthorName,
        string Text, 
        string TimeStamp 
    );