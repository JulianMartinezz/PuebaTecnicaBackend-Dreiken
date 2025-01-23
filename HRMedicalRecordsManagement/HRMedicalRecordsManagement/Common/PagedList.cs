namespace HRMedicalRecordsManagement.Common.PagedList;

public class PagedList<T>
{
    public IEnumerable<T> Items { get; set;}
    public int TotalCount {get; set;}
    public int CurrentPage {get; set;}
    public int TotalPages {get; set;}

    public PagedList(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        CurrentPage = currentPage;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}