using System;
using System.Collections.Generic;

namespace Library.API.Models
{
    public class AuthorUpdateResourceDTO : AuthorBaseResourceDTO
    {
        public ICollection<BookUpdateResourceDTO> Books { get; set; } = new List<BookUpdateResourceDTO>();
    }
}
