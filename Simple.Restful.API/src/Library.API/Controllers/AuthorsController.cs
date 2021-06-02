using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authorsRepository = _libraryRepository.GetAuthors();
            var authors = _mapper.Map<IEnumerable<AuthorDTO>>(authorsRepository);

            return Ok(authors);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId)
        {
            var authorsRepository = _libraryRepository.GetAuthor(authorId);
            var authors = _mapper.Map<AuthorDTO>(authorsRepository);

            if (authors == null)
                return NotFound();

            return Ok(authors);
        }

        [HttpPost()]
        public IActionResult CreateAuthor([FromBody] AuthorCreateResourceDTO author)
        {
            if (author == null)
                return BadRequest();

            var authorEntity = _mapper.Map<Author>(author);

            _libraryRepository.AddAuthor(authorEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception("Creating an author failed on save");
                //return StatusCode(500, "A problem happened with handling your request.")
            }

            var authorReturn = _mapper.Map<AuthorDTO>(authorEntity);

            return CreatedAtRoute("GetAuthor", new { authorId = authorReturn.Id }, authorReturn);
        }

        [HttpPost("{authorId}")]
        public IActionResult BlockAuthorCreation(Guid authorId)
        {
            if (_libraryRepository.AuthorExists(authorId))
                return new StatusCodeResult(StatusCodes.Status409Conflict);

            return NotFound();
        }

        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _libraryRepository.GetAuthor(authorId);
            if (authorFromRepo == null)
                return NotFound();

            _libraryRepository.DeleteAuthor(authorFromRepo);

            if (!_libraryRepository.Save())
                throw new Exception($"Deleting author {authorId} failed on save.");

            return NoContent();
        }
    }
}
