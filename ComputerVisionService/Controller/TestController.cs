using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ComputerVisionService.Controller
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("post")]
        public ActionResult PostAll()
        {
            return new JsonResult("OK");
        }

        [AllowAnonymous]
        [HttpGet("get")]
        public ActionResult GetAll()
        {

            string result = string.Empty;
            try
            {
                var info = new ProcessStartInfo("/bin/bash")
                {
                    Arguments = "python object_detection.py " + " " + "context.Message.AnimalRef" + " " + "context.Message.FileName",
                    RedirectStandardInput = false,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit(10000);
                    result = proc.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Script failed: " + result, ex);
            }

            // run script
            //var result = ScriptRunner.RunFromCmd("/app object_detection.py ", context.Message.AnimalRef + " " + context.Message.FileName);
            // retrieve results and send it to minio


            // send message to rabbitMQ to inform about results

            return new JsonResult(result);
        }
    }
}