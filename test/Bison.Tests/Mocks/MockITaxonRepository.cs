namespace Bison.Tests.Mocks;

using DTOs;
using Moq;

public class MockITaxonRepository
{
    public static Mock<ITaxonRepository> GetMock()
    {
        var taxon1 = new Taxon
        {
            TaxonId = 29,
            dwc_TaxonID = "MSTSNM:Arter:c68811f4-f785-ea11-aa77-501ac539d1ea",
            VernacularName = "Tophejre"
        };
        var taxon2 = new Taxon
        {
            TaxonId = 420,
            dwc_TaxonID = "Anders",
            VernacularName = null
        };

        var taxon1Dto = new TaxonOptionDTO(taxon1.dwc_TaxonID, taxon1.VernacularName);
        var taxons = new List<Taxon> { taxon1, taxon2 };
        var taxonsWithVerName = new List<Taxon> { taxon1 };
        var taxonDTOS = new List<TaxonOptionDTO> { taxon1Dto };
        var mock = new Mock<ITaxonRepository>();
        
        mock.Setup(m => m.GetTaxonById(It.IsAny<string>()))
            .ReturnsAsync((string taxonId) => taxons.FirstOrDefault(t => t.dwc_TaxonID == taxonId));

        mock.Setup(m => m.GetTaxonByVernacularName(It.IsAny<string>()))
            .ReturnsAsync((string vernacularName) => taxons.FirstOrDefault(t => t.VernacularName == vernacularName));
        
        mock.Setup(m => m.GetAllTaxons()).ReturnsAsync(taxons);

        mock.Setup(m => m.GetAllTaxonsWithVernacularName()).ReturnsAsync(taxonsWithVerName);

        mock.Setup(m => m.GetTaxonOptions()).ReturnsAsync(taxonDTOS);

        return mock;
    }
    
}