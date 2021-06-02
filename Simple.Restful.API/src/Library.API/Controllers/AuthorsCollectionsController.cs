using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace Library.API.Controllers
{
    [Route("api/authorsCollections")]
    public class AuthorsCollectionsController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public AuthorsCollectionsController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpPost()]
        public IActionResult CreateAuthor([FromBody] IEnumerable<AuthorCreateResourceDTO> authors)
        {
            if (authors == null || !authors.Any())
                return BadRequest();

            var authorEntities = _mapper.Map<IEnumerable<Author>>(authors);

            foreach (var authorEntity in authorEntities)
            {
                _libraryRepository.AddAuthor(authorEntity);
            }

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an author failed on save");
                //return StatusCode(500, "A problem happened with handling your request.")
            }
            var authorReturn = _mapper.Map<IEnumerable<AuthorDTO>>(authorEntities);

            var idsAsString = string.Join(",", authorReturn.Select(e => e.Id));

            return CreatedAtRoute("GetAuthorCollection", new { authorsId = idsAsString }, authorReturn);
        }

        [HttpGet("({authorsId})", Name = "GetAuthorCollection")]
        public IActionResult GetAuthorCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> authorsId)
        {
            if (authorsId == null || !authorsId.Any())
                return BadRequest();

            var authorEntities = _libraryRepository.GetAuthors(authorsId);

            if (authorsId.Count() != authorEntities.Count())
            {
                return NotFound();
            }

            var authorReturn = _mapper.Map<IEnumerable<AuthorDTO>>(authorEntities);

            return Ok(authorReturn);
        }
    }
}
