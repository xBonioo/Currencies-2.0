using Currencies.Abstractions.Contracts.Helpers;

namespace Currencies.Application.ModelDtos;

public class FilterDto
{
    public string? SearchPhrase { get; set; }
    public bool? IsActive { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = PropertyForQuery.AllowedPageSizes[0];
}
