using System;
using System.Collections.Generic;

namespace Library.API.Models
{
    public class AuthorCreateResourceDTO : AuthorBaseResourceDTO
    {
        public ICollection<BookCreateResourceDTO> Books { get; set; } = new List<BookCreateResourceDTO>();
    }
}
