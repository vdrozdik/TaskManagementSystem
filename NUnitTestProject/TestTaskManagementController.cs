using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.BusinessLayer;
using TaskManagementSystem.Controllers;
using TaskManagementSystem.Model;

namespace NUnitTestProject
{
    [TestFixture]
    public class TestTaskManagementController
    {
        private Mock<ILogger<TaskManagementController>> mockLogger;
        private Mock<IServiceBusHandler> mockServiceBusHundler;
        private Mock<IGlobalTaskCache> mockGlobalTaskCache;
        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<TaskManagementController>>();
            mockServiceBusHundler = new Mock<IServiceBusHandler>();
            mockGlobalTaskCache = new Mock<IGlobalTaskCache>();
            mockGlobalTaskCache.Setup(t => t.DisplayTasks()).Returns(new List<CreateTask>() {
                 new CreateTask{ TaskID = 1, Status = StatusTask.NotStarted, AssignedTo = "testUser_1", TaskName = "test_1"},
                 new CreateTask{ TaskID = 2, Status = StatusTask.InProgress, AssignedTo = "testUser_2", TaskName = "test_2"}
            });
        }


        [Test]
        public void ControllerDisplayTaskTest()
        {
            var controller = new TaskManagementController(mockLogger.Object, mockServiceBusHundler.Object, mockGlobalTaskCache.Object);

            var checkDisplayTask = (OkObjectResult)controller.Display();
            var result = checkDisplayTask.Value as List<CreateTask>;
             Assert.That(result.Count, Is.EqualTo(2));
        }


        [Test]
        public async Task ControllerUpdateTask_TaskIdNotExist_Test()
        {
            var controller = new TaskManagementController(mockLogger.Object, mockServiceBusHundler.Object, mockGlobalTaskCache.Object);

            var checkUpdateTask = await controller.Update(new UpdateTask { TaskID = 1, NewStatus = StatusTask.Completed});
            Assert.IsInstanceOf<BadRequestObjectResult>(checkUpdateTask);
        }
    }
}
