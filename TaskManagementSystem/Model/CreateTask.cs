using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Model
{
    public class CreateTask
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public StatusTask Status { get; set; }
        public string AssignedTo { get; set; }
        public string UpdatedBy { get; set; }
    }
}
