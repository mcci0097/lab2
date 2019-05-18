using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tasks.Model;

namespace Tasks.Services
{

    public interface ITaskService
    {
        IEnumerable<Task> GetAll();
        IEnumerable<Task> GetAllByFilter(IntervalDate intervalDate);
        Task GetById(int id);
        Task CreateTask(Task task);
        Task UpInsert(int id, Task task);
        Task Delete(int id);
        //IEnumerable<CommentFilterDTO> GetCommentsByFilter(String stringToSearch);
    }

    public class TaskService : ITaskService
    {
        private TaskDbContext context;
        public TaskService(TaskDbContext context)
        {
            this.context = context;
        }

        public Task CreateTask(Task task)
        {
            context.Tasks.Add(task);
            context.SaveChanges();
            return task;
        }

        public Task Delete(int id)
        {

            var existing = context.Tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null)
            {
                return null;
            }
            context.Tasks.Remove(existing);
            context.SaveChanges();
            return existing;
        }



        public IEnumerable<Task> GetAll()
        {
            return context.Tasks.Include(t => t.Comments);
        }

        public IEnumerable<Task> GetAllByFilter(IntervalDate intervalDate)
        {
            IQueryable<Task> result = context
              .Tasks
              .Include(f => f.Comments);
            if (intervalDate.start == null && intervalDate.end == null)
            {
                return context.Tasks;
            }
            if (intervalDate.start != null)
            {
                result = result.Where(t => t.Deadline >= intervalDate.start);
            }
            if (intervalDate.end != null)
            {
                result = result.Where(t => t.Deadline <= intervalDate.end);

            }

            return result;
        }

        public Task GetById(int id)
        {
            return context.
                Tasks.Include(t => t.Comments).
                FirstOrDefault(t => t.Id == id);
        }

        public Task UpInsert(int id, Task task)
        {
            var existing = context.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == id);

            if (existing == null)
            {
                context.Tasks.Add(task);
                context.SaveChanges();
                return task;
            }
            task.Id = id;
            DateTime dateToSet = DateTime.Now;

            if (task.Status.Equals(Task.State.Closed))
            {
                task.ClosedAt = dateToSet;
            }
            if (existing.Status.Equals(Task.State.Closed) && !task.Status.Equals(Task.State.Closed))
            {
                task.ClosedAt = null;

            }
            context.Tasks.Update(task);
            context.SaveChanges();
            return task;
        }

        //public IEnumerable<CommentFilterDTO> GetCommentsByFilter(String stringToSearch)
        //{

        //    IEnumerable<Task> all = context.Tasks.Include(t => t.Comments);
        //    CommentFilterDTO commentFilterDTO = new CommentFilterDTO();
        //    List<CommentFilterDTO> resultEnum = new List<CommentFilterDTO>();
        //    foreach (Task task in all)
        //    {
        //        foreach (Comment comment in task.Comments)
        //        {
        //            if (comment.Text.Contains(stringToSearch))
        //            {
        //                commentFilterDTO.Id = comment.Id;
        //                commentFilterDTO.Text = comment.Text;
        //                commentFilterDTO.Important = comment.Important;
        //                commentFilterDTO.TaskId = task.Id;
        //                resultEnum.Add(commentFilterDTO);
        //            }
        //        }
        //    }
        //    IEnumerable<CommentFilterDTO> send = resultEnum;
        //    return send;
        //}
    }
}

