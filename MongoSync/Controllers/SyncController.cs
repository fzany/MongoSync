using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoSync.Models;
using MongoSync.Services;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Labs.Search;

namespace MongoSync.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("RunSimulation/{query},{times}")]
        public async Task<ActionResult<List<User>>> RunSimulation(string query, int times)
        {
            try
            {
                BsonDocument pipeline = new BsonDocument {
  new BsonDocument("$search", new BsonDocument {
    {
      "index",
      "default"
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
  }) };

              var result = await _mongo._usersBson.AggregateAsync(pipeline);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*v
}*/

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
