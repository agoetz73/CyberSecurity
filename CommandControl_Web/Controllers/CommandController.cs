using CommandControl_Web.Models;
using System.Web.Http;
using CommandControl_Web.Services;
using System.Net.Http;

namespace CommandControl_Web.Controllers
{
    public class CommandController : ApiController
    {
        /// <summary>
        /// fetch command information
        /// </summary>
        private CommandRepository commandRepository;

        public CommandController()
        {
            this.commandRepository = new CommandRepository();
        }

        /// <summary>
        /// get all commands
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/command/get")]
        public Command[] Get()// Get()
        {
            return commandRepository.GetScriptsFromDB();//GetAllScripts();
        }

        /// <summary>
        /// get a specific command
        /// </summary>
        /// <param name="id">command id to fetch</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/command/get/{id}")]
        public Command Get(int id)
        {
            return commandRepository.GetScript(id);
        }

        /// <summary>
        /// create a new command
        /// </summary>
        /// <param name="command">command to add</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/command/post")]
        public HttpResponseMessage Post(Command command)
        {
            bool result = commandRepository.SaveScript(command);
            HttpRequestMessage request = new HttpRequestMessage();
            var configuration = new HttpConfiguration();
            var response = request.CreateResponse<Command>(System.Net.HttpStatusCode.Created, command, configuration);
            return response;
        }

        /// <summary>
        /// update a command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpPost]
        //[Route("api/client/post/{id}")]
        //public HttpResponseMessage Update(Command command, int id)
        //{
        //    bool result = commandRepository.UpdateScript(command, id);
        //    HttpRequestMessage request = new HttpRequestMessage();
        //    var configuration = new HttpConfiguration();
        //    var response = request.CreateResponse<Command>(System.Net.HttpStatusCode.OK, command, configuration);
        //    return response;
        //}
    }
}
