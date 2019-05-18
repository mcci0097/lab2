using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Tasks.Model;
using Tasks.Services;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        /// <summary>
        /// Gets all elements
        /// </summary>
        /// <returns>A list of all elements</returns>
        [HttpGet]
        public IEnumerable<Task> Get()
        {
            return taskService.GetAll();
        }

        /// <summary>
        /// Gets all tasks with filter
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /tasks
        ///     {
        ///         {
        ///             "start": "2019-06-14T23:57:19",
        ///             "end": "2019-06-14T23:57:19"
        ///         }   
        ///     }
        ///</remarks>
        /// <param name="intervalDate">Contains start and end date</param>
        /// <returns>A list of Task objects by filter</returns>
        [HttpGet("filter")]
        public IEnumerable<Task> GetByFilter([FromBody] IntervalDate intervalDate)
        {
            return taskService.GetAllByFilter(intervalDate);
        }

        //[HttpGet("comments")]
        //public IEnumerable<CommentFilterDTO> GetCommentsByFilter([FromQuery]String filter) {
        //    return taskService.GetCommentsByFilter(filter);
        //}


        /// <summary>
        /// Gets all tasks after id
        /// </summary>
        /// <param name="id">Optional, find tasks after id</param>
        /// <returns>A list of task objects </returns>
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var found = taskService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok();
        }


        /// <summary>
        /// Adds a new task 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /tasks
        ///     {
        ///             "id": 1,
        ///             "title": "string",
        ///             "description": "string",
        ///             "added": "2019-05-11T03:59:24.3376595",
        ///             "deadline": "2019-06-15T03:59:24.3407304",
        ///             "important": 2,
        ///             "state": 0,
        ///             "closeAt": "2019-06-15T03:59:24.341252",
        ///             "comments": [
        ///   	                {
        ///  	                    "id": 1,
        ///	                        "text": "string",
        ///	                        "important": true
        ///                      }
        ///             ]
        ///     }
        ///</remarks>
        /// <param name="task">Optional,Add a new task to the fields</param>
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] Task task)
        {
            taskService.CreateTask(task);
        }


        /// <summary>
        /// Updating a task
        /// </summary>
        /// <param name="id">Specify the id to be modified</param>
        /// <param name="task">Specify task object in JSON format </param>
        /// <returns>Returns the modified object</returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Task task)
        {
            var result = taskService.UpInsert(id, task);
            return Ok(result);
        }


        /// <summary>
        /// Deleting a task by id
        /// </summary>
        /// <param name="id">Specify id in URL to delete</param>
        /// <returns>Remove the task object from the list</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = taskService.Delete(id);
            if (existing == null)
            {
                return NotFound();
            }
            return Ok(existing);
        }
    }
}
