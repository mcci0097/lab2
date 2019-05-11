using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Model
{
    public class Task
    {
        public enum Importance {
            Low,
            Medium,
            High
        }
        public enum State {
            Open,
            InProgress,
            Closed
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Added { get; set; }
        public DateTime Deadline { get; set; }
        public Importance Imp { get; set; }
        public State Status { get; set; }
        public DateTime ClosedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
