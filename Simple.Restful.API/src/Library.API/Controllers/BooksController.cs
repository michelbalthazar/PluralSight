using System;
using System.Collections.Generic;
using AutoMapper;
using Library.API.Entities;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers
{
    [Route("api/authors/{authorId}/books")]
    public class BooksController : Controller
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public BooksController(ILibraryRepository libraryRepository, IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetBooksForAuthor(Guid authorId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
                return NotFound();

            var getBooksForAuthor = _libraryRepository.GetBooksForAuthor(authorId);
            var books = _mapper.Map<IEnumerable<BookDTO>>(getBooksForAuthor);

            return Ok(books);
        }

        [HttpGet("{bookId}", Name = "GetBookForAuthor")]
        public IActionResult GetBookForAuthor(Guid authorId, Guid bookId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
                return NotFound();

            var getBookForAuthor = _libraryRepository.GetBookForAuthor(authorId, bookId);

            if (getBookForAuthor == null)
                return NotFound();

            var books = _mapper.Map<BookDTO>(getBookForAuthor);

            return Ok(books);
        }

        [HttpPost()]
        public IActionResult CreateBookForAuthor(Guid authorId, [FromBody] BookCreateResourceDTO bookResourceDTO)
        {
            if (bookResourceDTO == null)
                return BadRequest();

            if (!_libraryRepository.AuthorExists(authorId))
                return NotFound();

            var bookEntity = _mapper.Map<Book>(bookResourceDTO);

            _libraryRepository.AddBookForAuthor(authorId, bookEntity);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Creating a book for author {authorId} failed on save");
                //return StatusCode(500, "A problem happened with handling your request.")
            }

            var book = _mapper.Map<BookDTO>(bookEntity);

            return CreatedAtRoute("GetBookForAuthor", new { authorId = authorId, bookId = book.Id }, book);
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(Guid authorId, Guid bookId)
        {
            if (!_libraryRepository.AuthorExists(authorId))
                return NotFound();

            var bookFromRepo = _libraryRepository.GetBookForAuthor(authorId, bookId);
            if (bookFromRepo == null)
                return NotFound();

            _libraryRepository.DeleteBook(bookFromRepo);

            if (!_libraryRepository.Save())
                throw new Exception($"Deleting book {bookId} from author {authorId} failed on save.");

            return NoContent();
        }

        [HttpPut("{bookId}")]
        public IActionResult UpdateBookForAuthor(Guid authorId, Guid bookId, [FromBody] BookUpdateResourceDTO bookResourceDTO)
        {
            if (bookResourceDTO == null)
                return BadRequest();

            if (!_libraryRepository.AuthorExists(authorId))
                return NotFound();

            var bookFromRepo = _libraryRepository.GetBookForAuthor(authorId, bookId);
            if (bookFromRepo == null)
                return NotFound();

            _mapper.Map<BookUpdateResourceDTO, Book>(bookResourceDTO, bookFromRepo);

            _libraryRepository.UpdateBookForAuthor(bookFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Creating a book for author {authorId} failed on save");
                //return StatusCode(500, "A problem happened with handling your request.")
            }

            var book = _mapper.Map<BookDTO>(bookFromRepo);

            return Ok(book);
        }

        [HttpPatch("{bookId}")]
        public IActionResult PathBookForAuthor(Guid authorId, Guid bookId, [FromBody] JsonPatchDocument<BookUpdateResourceDTO> bookResourceDTO)
        {
            if (bookResourceDTO == null)
            {
                return BadRequest();
            }

            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookFromRepo = _libraryRepository.GetBookForAuthor(authorId, bookId);
            if (bookFromRepo == null)
                return NotFound();

            var bookToPatch = _mapper.Map<BookUpdateResourceDTO>(bookFromRepo);

            bookResourceDTO.ApplyTo(bookToPatch, ModelState);

            if (bookToPatch.Description == bookToPatch.Title)
            {
                ModelState.AddModelError(nameof(BookUpdateResourceDTO),
                    "The provided description should be different from the title.");
            }

            TryValidateModel(bookToPatch);

            if (!ModelState.IsValid)
            {
                return new Library.API.Helpers.UnprocessableEntityObjectResult(ModelState);
            }

            _mapper.Map<BookUpdateResourceDTO, Book>(bookToPatch, bookFromRepo);

            _libraryRepository.UpdateBookForAuthor(bookFromRepo);

            if (!_libraryRepository.Save())
            {
                throw new Exception($"Patching book {bookId} for author {authorId} failed on save.");
            }

            var book = _mapper.Map<BookDTO>(bookFromRepo);

            return Ok(book);
        }
    }
}
