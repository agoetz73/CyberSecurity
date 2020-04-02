using CommandControl_Web.Models;
using System.Web.Http;
using CommandControl_Web.Services;
using System.Net.Http;

namespace CommandControl_Web.Controllers
{
    public class ExecutionController : ApiController
    {
        /// <summary>
        /// records of what happened when a command was run
        /// </summary>
        private ExecutionRepository executionRepository;

        public ExecutionController()
        {
            this.executionRepository = new ExecutionRepository();
        }

        /// <summary>
        /// list of all executions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/execution/get")]
        public Execution[] Get()// Get()
        {
            return executionRepository.GetExecutions();
        }

        /// <summary>
        /// get a specific execution
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/execution/get/{id}")]
        public Execution Get(int id)
        {
            return executionRepository.GetExecution(id);
        }

        /// <summary>
        /// post a new exuection result
        /// </summary>
        /// <param name="execution"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/execution/post")]
        public HttpResponseMessage Post(Execution execution)
        {
            bool result = executionRepository.NewExecution(execution);
            HttpRequestMessage request = new HttpRequestMessage();
            var configuration = new HttpConfiguration();
            var response = request.CreateResponse<Execution>(System.Net.HttpStatusCode.Created, execution, configuration);
            return response;
        }
    }
}