using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers
{
    public class RequestParams
    {
        public string? SearchPhrase { get; set; }
        public int BudgetId { get; set; }
        public DateTime DateFrom { get; set; } = DateTime.MinValue;
        public DateTime DateTo { get; set; } = DateTime.Now;

        const int maxPageSize = 25;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 5;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }
    }
}
