using EXAM_SYSTEM_API.Application.Shared.Responses;
using System.Text.Json.Serialization;

namespace EXAM_SYSTEM_API.Application.Shared;

public class PaginationResponse<T> : ResponseBase
{
    [JsonPropertyOrder(1)]
    public Pagin Pagin { get; set; } = new();

    [JsonPropertyOrder(2)]
    public List<T> Data { get; set; } = new();

    public PaginationResponse(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Pagin = new Pagin
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalRows = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        Data = items;
        StatusCode = 200;
        Success = true;
        Message = "Success";
    }
}

public class Pagin
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalRows { get; set; }
    public int TotalPages { get; set; }
}