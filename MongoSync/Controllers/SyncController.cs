using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoSync.Models;
using MongoSync.Services;
using Newtonsoft.Json;

namespace MongoSync.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class SyncController : ControllerBase
    {
        private readonly MongoService _mongo;
        private readonly ILogger<SyncController> _logger;

        public SyncController(ILogger<SyncController> logger, MongoService mongo)
        {
            _logger = logger;
            _mongo = mongo;
        }

        [HttpGet("TestConnection")]
        public async Task<ActionResult> TestConnection()
        {
            try
            {

                var count = await _mongo._users.CountDocumentsAsync(d => true);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var users = await _mongo._users.Find(d => true).FirstOrDefaultAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("RunSimulation")]
        public async Task<ActionResult<List<User>>> RunSimulation([FromQuery] string query)
        {
            try
            {
                var result = await _mongo._users.Find(d => d.FirstName.Contains(query) || d.LastName.Contains(query)).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Perform Search
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("RunSearch")]
        public async Task<ActionResult> RunSearch([FromQuery] string query)
        {
            try
            {
                PipelineDefinition<User, BsonDocument> pipeline = new[] {
                   new BsonDocument("$search", new BsonDocument {
    {
      "index",
      "NameSearch"
    },
    {
      "text",
      new BsonDocument {
        {
          "query",
          query
        },
        {
          "path",
          new BsonDocument("wildcard", "*")
        }
      }
    }
  })
                };

                var result = await _mongo._users.AggregateAsync(pipeline);
                var resultList = await result.ToListAsync();

                //List<User> outPutUsers = new List<User>();

                //foreach (var userBson in resultList)
                //{
                //   // Debug.WriteLine(userBson);
                //    outPutUsers.Add(BsonSerializer.Deserialize<User>(userBson));
                //}
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("Upload")]
        public async Task<ActionResult> Upload()
        {
            try
            {
                var file = await System.IO.File.ReadAllTextAsync("/Users/olorunfemi/Downloads/User.json");
                var data = JsonConvert.DeserializeObject<List<User>>(file);

                //Insert into the database.
                await _mongo._users.InsertManyAsync(data);
                return Ok("Done");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
