using NUnit.Framework;
using System;
using TaskManagementSystem.BusinessLayer;
using TaskManagementSystem.Model;

namespace NUnitTestProject
{
    [TestFixture]
    public class TestGlobalTaskCache
    {
        [Test]
        public void Check_AddTask()
        {
            GlobalTaskCache globalTaskCache = new GlobalTaskCache();
            globalTaskCache.AddTask(new CreateTask { 
            TaskID = 1,
            TaskName = "test",
            Status = 0,    
            });
            Assert.That(globalTaskCache.DisplayTasks().Count, Is.EqualTo(1));
        }

        [TestCase(1)]
        public void Check_UpdateTask(int taskId)
        {
            GlobalTaskCache globalTaskCache = new GlobalTaskCache();
            globalTaskCache.AddTask(new CreateTask
            {
                TaskID = taskId,
                TaskName = "test",
                Status =(StatusTask) 0,
            });
            globalTaskCache.UpdateTask(new UpdateTask
            {
                TaskID = taskId,
                NewStatus = (StatusTask)1,
                UpdatedBy = "TestUser"

            });


            Assert.That(globalTaskCache.DisplayTask(taskId).Status, Is.EqualTo((StatusTask)1));
            Assert.That(globalTaskCache.DisplayTask(taskId).UpdatedBy, Is.EqualTo("TestUser"));
        }
        [TestCase(1, 2)]
        public void Check_UpdateTask_TaskIdNotExist(int taskIdCreate, int taskIdUpdate)
        {
            GlobalTaskCache globalTaskCache = new GlobalTaskCache();
            globalTaskCache.AddTask(new CreateTask
            {
                TaskID = taskIdCreate,
                TaskName = "test",
                Status = (StatusTask)0,
            });
            globalTaskCache.UpdateTask(new UpdateTask
            {
                TaskID = taskIdUpdate,
                NewStatus = (StatusTask)1,
                UpdatedBy = "TestUser"

            });
        }

        [TestCase(1, 2)]
        public void Check_DisplayTask_TaskIdNotExist(int taskIdCreate, int taskIdDisplay)
        {
            GlobalTaskCache globalTaskCache = new GlobalTaskCache();
            globalTaskCache.AddTask(new CreateTask
            {
                TaskID = taskIdCreate,
                TaskName = "test",
                Status = (StatusTask)0,
            });

            Assert.That(globalTaskCache.DisplayTask(taskIdDisplay), Is.EqualTo(null));
        }
    }
}
