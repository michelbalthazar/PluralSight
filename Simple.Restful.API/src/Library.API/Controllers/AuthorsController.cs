using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
using Library.API.Helpers;
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
        private readonly IUrlHelper _urlHelper;

        public AuthorsController(ILibraryRepository libraryRepository, IMapper mapper, IUrlHelper urlHelper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name ="GetAuthors")]
        public IActionResult GetAuthors([FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsRepository = _libraryRepository.GetAuthors(authorsResourceParameters);

            var previousPageLink = authorsRepository.HasPrevious ?
                CreateAuthorsResourceUri(authorsResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = authorsRepository.HasNext ?
                CreateAuthorsResourceUri(authorsResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = authorsRepository.TotalCount,
                pageSize = authorsRepository.PageSize,
                currentPage = authorsRepository.CurrentPage,
                totalPages = authorsRepository.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var authors = _mapper.Map<IEnumerable<AuthorDTO>>(authorsRepository);

            return Ok(authors);
        }


        private string CreateAuthorsResourceUri(
            AuthorsResourceParameters authorsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAuthors",
                      new
                      {
                          searchQuery = authorsResourceParameters.SearchQuery,
                          genre = authorsResourceParameters.Genre,
                          pageNumber = authorsResourceParameters.PageNumber - 1,
                          pageSize = authorsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAuthors",
                      new
                      {
                          searchQuery = authorsResourceParameters.SearchQuery,
                          genre = authorsResourceParameters.Genre,
                          pageNumber = authorsResourceParameters.PageNumber + 1,
                          pageSize = authorsResourceParameters.PageSize
                      });

                default:
                    return _urlHelper.Link("GetAuthors",
                    new
                    {
                        searchQuery = authorsResourceParameters.SearchQuery,
                        genre = authorsResourceParameters.Genre,
                        pageNumber = authorsResourceParameters.PageNumber,
                        pageSize = authorsResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor([FromRoute] Guid authorId)
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
