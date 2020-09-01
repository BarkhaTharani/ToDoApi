using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Data.Dtos;
using ToDoApi.Data.Entities;
using ToDoApi.Helpers;
using ToDoApi.Mappers;

namespace ToDoApi.Controllers {

    [ApiController]
    [Route ("api/[controller]")]
    public class ToDoController : ControllerBase {
        private readonly ToDoContext _context;

        public ToDoController (ToDoContext context) {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll () {
            return _context.ToDoItems.ToList ();
        }

        [Authorize]
        [HttpGet ("{id}", Name = "GetTodo")]
        public async Task<ActionResult<TodoItemDto>> GetItem (long id) {
            var entity = await _context.ToDoItems.FindAsync (id);

            if (entity == null)
                return NotFound ();
            return (ToDoItemToDtoMapper.MapEntity (entity));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> AddItem (TodoItemDto dto) {
            if (TodoItemNameExists (dto.name))
                return Conflict ();

            var mappedItem = ToDoItemToDtoMapper.MapDto (dto);
            _context.ToDoItems.Add (mappedItem);
            await _context.SaveChangesAsync ();

            var mappedDto = ToDoItemToDtoMapper.MapEntity (mappedItem);
            return CreatedAtAction (nameof (GetItem), new { id = mappedDto.id }, mappedDto);
        }

        [Authorize]
        [HttpPut ("{id}")]
        public async Task<ActionResult<ToDoItem>> EditItem (long id, TodoItemDto dto) {
            var entity = await _context.ToDoItems.FindAsync (id);

            if (entity == null) {
                return NotFound ();
            }

            if (TodoItemNameExists (dto.name) && entity.id != id)
                return Conflict ();

            entity.name = dto.name;
            entity.isComplete = dto.isComplete;

            _context.Entry (entity).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync ();
            } catch (DbUpdateConcurrencyException ex) {
                if (!TodoItemExists (id)) {
                    return NotFound ();
                }
                throw;
            }

            return NoContent ();
        }

        [Authorize]
        [HttpDelete ("{id}")]
        public async Task<ActionResult<TodoItemDto>> DeleteTodoItem (long id) {
            var entity = await _context.ToDoItems.FindAsync (id);
            if (entity == null) {
                return NotFound ();
            }

            _context.ToDoItems.Remove (entity);
            await _context.SaveChangesAsync ();

            return ToDoItemToDtoMapper.MapEntity (entity);
        }

        private bool TodoItemExists (long id) =>
            _context.ToDoItems.Any (e => e.id == id);

        private bool TodoItemNameExists (string name) =>
            _context.ToDoItems.Any (e => e.name.ToLower () == name.ToLower ());

    }
}