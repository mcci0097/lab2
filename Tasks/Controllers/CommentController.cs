using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasks.Model;
using Tasks.Services;

namespace Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        /// <summary>
        /// Gets all comments filtered by a string
        /// </summary>
        /// <param name="filter">The keyword used to search by</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CommentFilterDTO> GetCommentsByFilter([FromQuery]String filter)
        {
            return commentService.GetCommentsByFilter(filter);
        }
    

        //// GET: api/Comment/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Comment
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT: api/Comment/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
