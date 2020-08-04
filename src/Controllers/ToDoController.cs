using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Data.Models;

namespace ToDoApi.Controllers {

    [ApiController]
    [Route ("api/[controller]")]
    public class ToDoController : ControllerBase {
        private readonly ToDoContext _context;

        public ToDoController (ToDoContext context) {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ToDoItem>> GetAll () {
            return _context.ToDoItems.ToList ();
        }

        [HttpGet ("{id}", Name = "GetTodo")]
        public async Task<ActionResult<ToDoItem>> GetItem (long id) {
            var item = await _context.ToDoItems.FindAsync (id);

            if (item == null)
                return NotFound ();
            return (item);
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> AddItem (ToDoItem item) {
            if(TodoItemNameExists(item.name))
                return Conflict();

            _context.ToDoItems.Add (item);
            await _context.SaveChangesAsync ();

            return CreatedAtAction (nameof (GetItem), new { id = item.id }, item);
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult<ToDoItem>> EditItem (long id, ToDoItem item) {
            if (id != item.id)
                return BadRequest ();

            _context.Entry (item).State = EntityState.Modified;

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

        [HttpDelete ("{id}")]
        public async Task<ActionResult<ToDoItem>> DeleteTodoItem (long id) {
            var todoItem = await _context.ToDoItems.FindAsync (id);
            if (todoItem == null) {
                return NotFound ();
            }

            _context.ToDoItems.Remove (todoItem);
            await _context.SaveChangesAsync ();

            return todoItem;
        }

        private bool TodoItemExists (long id) =>
            _context.ToDoItems.Any (e => e.id == id);

        private bool TodoItemNameExists (string name) =>
            _context.ToDoItems.Any (e => e.name == name);

    }
}