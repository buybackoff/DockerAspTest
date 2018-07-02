using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DockerAspTest
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //var drives = System.IO.DriveInfo.GetDrives().Select(d => d.ToString());
            //return drives; // 
            if (!System.IO.File.Exists("/localdata/text.txt"))
            {
                System.IO.File.WriteAllText("/localdata/text.txt", "Hello, Docker!");
            }
            return Directory.GetFileSystemEntries("/localdata", "*", SearchOption.AllDirectories);
            // return System.IO.Directory.GetDirectories("/datavol", "*", System.IO.SearchOption.AllDirectories);
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        //// PUT api/<controller>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
