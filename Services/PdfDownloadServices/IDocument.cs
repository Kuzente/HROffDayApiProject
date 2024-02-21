using Core.DTOs.OffDayDTOs.ReadDtos;
using QuestPDF.Infrastructure;

namespace Services.PdfDownloadServices;

public interface IDocument
{
    DocumentMetadata GetMetadata();
    DocumentSettings GetSettings();
    void Compose(IDocumentContainer container,  ReadApprovedOffDayFormExcelExportDto dto);
}