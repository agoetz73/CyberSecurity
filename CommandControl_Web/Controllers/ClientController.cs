using CommandControl_Web.Models;
using System.Web.Http;
using CommandControl_Web.Services;
using System.Net.Http;

namespace CommandControl_Web.Controllers
{
    /// <summary>
    /// Fetch client information
    /// </summary>
    public class ClientController : ApiController
    {
        private ClientRepository clientRepository;

        public ClientController()
        {
            this.clientRepository = new ClientRepository();
        }

        /// <summary>
        /// Get all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/client/get")]
        public Client[] Get()
        {
            return clientRepository.GetClients();
        }

        /// <summary>
        /// get specific client
        /// </summary>
        /// <param name="id">client id to fetch</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/client/get/{id}")]
        public Client Get(int id)// Get()
        {
            if (id < 0)
                return null;
            else
                return clientRepository.GetClient(id);
        }

        /// <summary>
        /// update a client
        /// </summary>
        /// <param name="client">client to update</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/client/post")]
        public HttpResponseMessage Post(Client client)
        {
            Client c = clientRepository.ClientKnown(client.ClientName);
            HttpRequestMessage request = new HttpRequestMessage();
            var configuration = new HttpConfiguration();
            HttpResponseMessage response = request.CreateResponse<Client>(System.Net.HttpStatusCode.InternalServerError, client, configuration);

            if (c.Id == -1)
            {
                bool result = clientRepository.NewClient(client);
                response = request.CreateResponse<Client>(System.Net.HttpStatusCode.Created, client, configuration);
            }
            else
            {
                client.Id = c.Id;
                {
                    bool result = clientRepository.UpdateClient(client);
                    response = request.CreateResponse<Client>(System.Net.HttpStatusCode.Accepted, client, configuration);
                }
            }

            return response;
        }
    }
}