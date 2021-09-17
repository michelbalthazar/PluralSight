using System;
using System.Collections.Generic;

namespace Library.API.Helpers
{
    public class AuthorsResourceParameters
    {
        const int MaxPageSize = 20;
        private int _pageSize = 10;

        public string Genre { get; set; }
        public string SearchQuery { get; set; }
        public IEnumerable<Guid> AuthorIds { get; set; } = new List<Guid>();
        
        public int PageNumber { get; set; }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
    }
}
