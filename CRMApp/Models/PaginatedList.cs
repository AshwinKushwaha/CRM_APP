namespace CRMApp.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }
        public int EndPage { get; private set; }



        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);

            int startPage = PageIndex - 3;
            int endPage = pageIndex + 2;


            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > 5)
                {
                    startPage = endPage - 4;
                }
            }

            StartPage = startPage;
            EndPage = endPage;
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;



    }
}
