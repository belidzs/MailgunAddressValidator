using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

namespace MailgunAddressValidator
{
    public static class Validator
    {
        private const string MailgunBaseUrl = "https://api.mailgun.net/v3";
        private const string ValidatorResource = "/address/validate";
        
        public static ValidationResult Validate(string email, string apikey, int timeout = 2000)
        {
            IRestResponse<ValidationResult> result = GetClient(apikey).Execute<ValidationResult>(GetRequest(email, timeout));
            EvaluateResult(result);
            return result.Data;
        }

        public static async Task<ValidationResult> ValidateAsync(string email, string apikey, int timeout = 2000)
        {
            IRestResponse<ValidationResult> result = await GetClient(apikey).ExecuteTaskAsync<ValidationResult>(GetRequest(email, timeout));
            EvaluateResult(result);
            return result.Data;
        }

        private static void EvaluateResult(IRestResponse<ValidationResult> result)
        {
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            if (result.ErrorException != null)
            {
                throw result.ErrorException;
            }
        }

        private static RestClient GetClient(string apikey)
        {
            RestClient client = new RestClient()
            {
                BaseUrl = new Uri(MailgunBaseUrl),
                Authenticator = new HttpBasicAuthenticator("api", apikey)
            };
            return client;
        }

        private static RestRequest GetRequest(string email, int timeout)
        {
            RestRequest request = new RestRequest(ValidatorResource)
            {
                Timeout = timeout
            };
            request.AddParameter("address", email);
            return request;
        }
    }
}
