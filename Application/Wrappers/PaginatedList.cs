using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public string SearchPhrase { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageSize = pageSize;
            PageNumber = pageNumber;
            ItemsFrom = pageSize * (pageNumber - 1);
            ItemsTo = ItemsFrom + pageNumber - 1;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public static PaginatedList<T> CreateAsync(List<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
