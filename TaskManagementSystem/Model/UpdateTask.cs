using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Model
{
    public class UpdateTask
    {
        [Required]
        public int TaskID { get; set; }
        [Required]
        public StatusTask NewStatus { get; set; }
        public string UpdatedBy { get; set; }
    }
}
