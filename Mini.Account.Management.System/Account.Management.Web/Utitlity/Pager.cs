namespace Account.Management.Web.Utitlity
{
    public class Pager
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public Pager(int totalItems, int page, int pageSize = 10)
        {
            TotalItems = totalItems;
            CurrentPage = page < 1 ? 1 : page;
            PageSize = pageSize;
        }

        public int StartIndex => (CurrentPage - 1) * PageSize;
    }
}
