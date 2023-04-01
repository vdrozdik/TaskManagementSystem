using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementSystem.Amqp;
using TaskManagementSystem.BusinessLayer;

using TaskManagementSystem.Model;
using TaskManagementSystem.RabbitMq;

namespace TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskManagementController : ControllerBase
    {
        private readonly ILogger<TaskManagementController> logger;
        private readonly IServiceBusHandler serviceBusHandler;
        private readonly IGlobalTaskCache globalTaskCache;
        public TaskManagementController(ILogger<TaskManagementController> logger, IServiceBusHandler serviceBusHandler, IGlobalTaskCache globalTaskCache)
        {
            this.logger = logger;
            this.serviceBusHandler = serviceBusHandler;
            this.globalTaskCache = globalTaskCache;
        }

        [HttpGet]
        [Route("display")]
        public ActionResult Display()
        {
            return Ok(globalTaskCache.DisplayTasks());
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [Route("update")]
        public async Task<ActionResult> Update([FromBody] UpdateTask updateTask)
        {
            if (globalTaskCache.DisplayTask(updateTask.TaskID) != null)
            {
                var message = new AmqpMessage
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    Data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(updateTask)),
                    Rout = Globals.rabbitReplyTo,
                    RabbitQueueRequest = Globals.rabbitQueueRequest
                };
                await serviceBusHandler.SendMessage(message);
                globalTaskCache.UpdateTask(updateTask);
                return Ok();
            }
            else
                return BadRequest("TaskID not exist");
        }
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [Route("add")]
        public ActionResult Add([FromBody] AddTask addTask)
        {
            globalTaskCache.AddTask(new CreateTask()
            {
                TaskID = globalTaskCache.GetGlobalTaksId(),
                AssignedTo = addTask.AssignedTo,
                Description = addTask.Description,
                Status = addTask.Status,
                TaskName = addTask.TaskName
            });
            return Ok();
        }
    }
}
