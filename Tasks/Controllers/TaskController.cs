using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Tasks.Model;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private TaskDbContext context;
        public TaskDbContext Context { get => context; set => context = value; }

        public TaskController(TaskDbContext context)
        {
            this.context = context;
        }

        // GET: api/Task
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return Context.Tasks.Include(t=>t.Comments)Include(t => t.Description);
        }

        // GET: api/Task/filter
        [HttpGet("filter")]
        public IEnumerable<Task> GetByFilter([FromBody] IntervalDate intervalDate)
        {

            IQueryable<Task> list = Context.Tasks.Include(t=>t.Comments).Include(t=>t.Description);
            IList<Task> result = new List<Task>() { };

            foreach (Task task in list)
            {
                if (task.Deadline > intervalDate.start && task.Deadline < intervalDate.end)
                {
                    System.Diagnostics.Debug.WriteLine(task.Status);
                    result.Add(task);
                }
            }

            foreach (Task task in result)
            {
                System.Diagnostics.Debug.WriteLine(task.Title);
            }
            System.Diagnostics.Debug.WriteLine("end");

            return result;
        }

        // GET: api/Task/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Task
        [HttpPost]
        public IActionResult Post([FromBody] Task value)
        {
            Context.Tasks.Add(value);
            Context.SaveChanges();
            return Ok();
        }

        // PUT: api/Task/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Task task)
        {                        
            var existing = Context.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id);
            
            if (existing == null)
            {
                Context.Tasks.Add(task);
                Context.SaveChanges();
                return Ok(task);
            }
            task.Id = id;
            DateTime dateToSet = DateTime.Now;

            if (task.Status.Equals(Task.State.Closed))
            {
                task.ClosedAt = dateToSet;
            }
            if (existing.Status.Equals(Task.State.Closed) && !task.Status.Equals(Task.State.Closed))
            {
                task.ClosedAt = DateTime.MinValue;
            }
            Context.Tasks.Update(task);
            Context.SaveChanges();
            return Ok(task);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = Context.Tasks.FirstOrDefault(product => product.Id == id);
            if (existing == null)
            {
                return NotFound();
            }
            Context.Tasks.Remove(existing);
            Context.SaveChanges();
            return Ok();
        }
    }
}
