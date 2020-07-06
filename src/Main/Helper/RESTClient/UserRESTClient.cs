using RestSharp;
using System.Configuration;

namespace OnlineOrderInfo
{
    public class UserRestClient:RestClient
    {
        public UserRestClient(string parameterId,string resource,string parameterValue)
        {
            ParameterId = parameterId;
            Resource = resource;
            Client = new RestClient("https://demo.tacitdev.ca/webordering/api/v1");
            //Client.Timeout = -1;

            Request = new RestRequest(resource + "/{" + parameterId + "}", Method.GET);
            Request.AddUrlSegment(parameterId, parameterValue);
            Request.AddHeader("Content-Type", ConfigurationManager.AppSettings["Content-Type"].ToString());
            Request.AddHeader("Authorization", ConfigurationManager.AppSettings["Authorization"].ToString());
            Request.AddHeader("App-Key", ConfigurationManager.AppSettings["App-Key"].ToString());
            Request.AddHeader("Site-Name", ConfigurationManager.AppSettings["Site-Name"].ToString());
            Request.AddHeader("App-Language", ConfigurationManager.AppSettings["App-Language"].ToString());
        }

        private string ParameterId { get; set; }
        private string Resource { get; set; }
        internal RestRequest Request;
        internal RestClient Client;

    }
}
