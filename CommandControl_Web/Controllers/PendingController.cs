using CommandControl_Web.Models;
using System.Web.Http;
using CommandControl_Web.Services;
using System.Net.Http;

namespace CommandControl_Web.Controllers
{
    public class PendingController : ApiController
    {
        /// <summary>
        /// list of commands to go out to clients
        /// </summary>
        private PendingRepository pendingRepository;

        public PendingController()
        {
            this.pendingRepository = new PendingRepository();
        }

        /// <summary>
        /// get all pendings
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/pending/get")]
        public Pending[] Get()// Get()
        {
            return pendingRepository.GetPendings();
        }

        /// <summary>
        /// get all pendings for a specific client
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/pending/get/{id}")]
        public Pending[] Get(int id)// Get()
        {
            return pendingRepository.GetPending(id);
        }

        /// <summary>
        /// post a new pending
        /// </summary>
        /// <param name="pending"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/pending/post")]
        public HttpResponseMessage Post(Pending pending)
        {
            bool result = pendingRepository.NewPending(pending);
            HttpRequestMessage request = new HttpRequestMessage();
            var configuration = new HttpConfiguration();
            var response = request.CreateResponse<Pending>(System.Net.HttpStatusCode.Created, pending, configuration);
            return response;
        }

        /// <summary>
        /// update a pending
        /// </summary>
        /// <param name="pending"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/pending/post/{id}")]
        public HttpResponseMessage Post(int id)
        {
            bool result = pendingRepository.UpdatePending(id);
            HttpRequestMessage request = new HttpRequestMessage();
            var configuration = new HttpConfiguration();
            var response = request.CreateResponse<int>(System.Net.HttpStatusCode.Created, id, configuration);
            return response;
        }
    }
}