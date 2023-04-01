using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.BusinessLayer
{
    public interface IGlobalTaskCache
    {
        int GetGlobalTaksId();
        void AddTask(CreateTask task);
        List<CreateTask> DisplayTasks();
        void DeleteTask(CreateTask task);
        void UpdateTask(UpdateTask task);
        CreateTask DisplayTask(int taskId);
    }
}
