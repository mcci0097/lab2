using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Model
{
    public class TaskDbSeeder
    {
        public static void Initialize(TaskDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Tasks.Any())
            {
                return;   // DB has been seeded
            }

            context.Tasks.AddRange(
                new Task
                {
                    Title = "Make money",
                    Added = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(35),
                    Imp = Task.Importance.High,
                    Status = Task.State.InProgress,
                    ClosedAt = DateTime.Now.AddDays(35),
                },

                new Task
                {
                    Title = "Get bitches",
                    Added = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(14),
                    Imp = Task.Importance.Low,
                    Status = Task.State.Open,
                    ClosedAt = DateTime.Now.AddDays(14),
                }
            );
            context.SaveChanges();
        }
    }
}
