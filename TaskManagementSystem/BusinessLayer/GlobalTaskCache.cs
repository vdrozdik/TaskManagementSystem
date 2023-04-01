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
        private readonly object Lock = new object();
        private int getGlobalTaskId
        {
            get
            {
                lock (Lock)
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
                //if (_getChache == 0)
                //{
                //    result = currentTask;
                //}
                //else
                //{
                    //lock (Lock)
                    //{
                        result = currentTask;
                    //}
               // }
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
                objTask.Status = task.NewStatus;
                objTask.UpdatedBy = task.UpdatedBy;
            }
        }
    }
}
