using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationCore.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet("[Action]")]
        public async Task<IEnumerable<TodoItem>> GetTodosAsync()
        {
            //Console.Write(_context.TodoItems.ToList());
            return await _context.TodoItems.ToListAsync();
        }

        [HttpGet("[Action]")]
        public async Task<TodoItem> GetTodoAsync(int id)
        {
            //Console.Write(_context.TodoItems.Find(id));
            return  await _context.TodoItems.FindAsync(id);
           
        }

        [HttpPost]
        [Route("InsertData1")]
        public async Task<IActionResult> InsertData([FromBody] TodoItem todoItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _context.AddAsync<TodoItem>(todoItem);
                    int res = _context.SaveChanges();

                    if (res > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("InsertData")]
        public async Task<IActionResult> InsertData(int Id, string Name, bool IsComplete)
        {
            try
            {
                TodoItem todoItem = new TodoItem();
                todoItem.Id = Id;
                todoItem.Name = Name;
                todoItem.IsComplete = IsComplete;
                var result = await _context.AddAsync<TodoItem>(todoItem);
                int res = _context.SaveChanges();
                if (res > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPut]
        [Route("UpdateData")]
        public async Task<IActionResult> UpdateData(int Id, string Name, bool IsComplete)
        {
            try
            {
                var result = _context.TodoItems.Find(Id);
                result.Name = Name;
                result.IsComplete = IsComplete;
                var data = await _context.AddAsync<TodoItem>(result);
                int res = _context.SaveChanges();
                if (res > 0)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpPut]
        [Route("UpdateData1")]
        public async Task<IActionResult> UpdateData(int Id, [FromBody]TodoItem todoItem)
        {
            try
            {
                var result = _context.TodoItems.Find(Id);
                result.Name = todoItem.Name;
                result.IsComplete = todoItem.IsComplete;
                var data = await _context.AddAsync<TodoItem>(result);
              int res =  _context.SaveChanges();

                if (res>0)
                {
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        [Route("DeleteTodo")]
        [EnableCors("AllowOrigin")]
        public int RemoveTodo(int Id)
        {
            var result = _context.TodoItems.Find(Id);
            _context.TodoItems.Remove(result);
          return  _context.SaveChanges();
        }
    }
}
