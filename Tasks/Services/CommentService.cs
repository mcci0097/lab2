using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Model;

namespace Tasks.Services
{

    public interface ICommentService
    {
        IEnumerable<CommentFilterDTO> GetCommentsByFilter(String stringToSearch);
    }

    public class CommentService : ICommentService
    {
        private TaskDbContext context;
        public CommentService(TaskDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<CommentFilterDTO> GetCommentsByFilter(String stringToSearch)
        {

            IEnumerable<Task> all = context.Tasks.Include(t => t.Comments);
            CommentFilterDTO commentFilterDTO;
            List<CommentFilterDTO> resultEnum = new List<CommentFilterDTO>();
            foreach (Task task in all)
            {
                foreach (Comment comment in task.Comments)
                {
                    if (comment.Text.Contains(stringToSearch))
                    {
                        commentFilterDTO = new CommentFilterDTO();
                        commentFilterDTO.Id = comment.Id;
                        commentFilterDTO.Text = comment.Text;
                        commentFilterDTO.Important = comment.Important;
                        commentFilterDTO.TaskId = task.Id;
                        resultEnum.Add(commentFilterDTO);
                    }
                }
            }
            IEnumerable<CommentFilterDTO> send = resultEnum;
            return send;
        }
    }
}
