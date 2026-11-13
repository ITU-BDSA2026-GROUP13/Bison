namespace Bison.DTOs;


public record ObservationDTO
    (
        long ObservationId,
        string AuthorId,
        string dwc_TaxonId,
        string AuthorName, 
        string TaxonName,
        string Text,
        string TimeStamp,
<<<<<<< HEAD:src/Bison.Core/DTOs/ObservationDTO.cs
        string? ImageUrl
=======
        string? PictureFileName = null
>>>>>>> d2852d9 (Feat. The commit is to make sure that the migrations comes along):src/Bison.Razor/DTOs/ObservationDTO.cs
    ); 