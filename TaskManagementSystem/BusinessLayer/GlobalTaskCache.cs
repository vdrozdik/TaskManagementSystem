using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Model;



namespace TaskManagementSystem.BusinessLayer
{
    public class GlobalTaskCache : IGlobalTaskCache
    {
        private int MaxValue = 2147483647;
        private static List<CreateTask> currentTask = new List<CreateTask>();
        private readonly object LockMaxValue = new object();
        private readonly object LockGetTask = new object();
        private int getGlobalTaskId
        {
            get
            {
                lock (LockMaxValue)
                {
                    return MaxValue -- ;
                }
            }
        }
        private List<CreateTask> currentTaskCache
        {
            get
            {
                List<CreateTask> result;

                lock (LockGetTask)
                {
                    result = currentTask;
                }

                return result;
            }
        }
        public void AddTask(CreateTask task)
        {
            currentTask.Add(task);
        }

        public void DeleteTask(CreateTask task)
        {
            currentTask.RemoveAll(t => t.TaskID == task.TaskID);
        }

        public List<CreateTask> DisplayTasks()
        {
            return currentTaskCache;
        }
        public CreateTask DisplayTask(int taskId)
        {
            return currentTaskCache.Where(t=>t.TaskID == taskId)?.FirstOrDefault();
        }

        public int GetGlobalTaksId()
        {
            return getGlobalTaskId;
        }

        public void UpdateTask(UpdateTask task)
        {
            var objTask = currentTask.FirstOrDefault(x => x.TaskID == task.TaskID);
            if (objTask != null)
            {
                lock (LockGetTask)
                {
                    objTask.Status = task.NewStatus;
                    objTask.UpdatedBy = task.UpdatedBy;
                }
            }
        }
    }
}
