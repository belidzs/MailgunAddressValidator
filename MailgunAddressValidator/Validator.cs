// <copyright file="Validator.cs" company="Balázs Keresztury">
// Copyright (c) Balázs Keresztury. All rights reserved.
// </copyright>

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MailgunAddressValidator
{
    /// <summary>
    /// Contains static functions to validate an e-mail address using Mailgun's e-mail validation service.
    /// </summary>
    public static class Validator
    {
        private const string MailgunBaseUrl = "https://api.mailgun.net";
        private const string ValidatorResource = "/v3/address/validate";
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            },
        };

        /// <summary>
        /// Validates an e-mail address in a syncronous fashion.
        /// </summary>
        /// <param name="email">E-mail address to validate.</param>
        /// <param name="apikey">Mailgun e-mail validation API key.</param>
        /// <param name="timeout">Max. time to wait for the call to complete (in ms).</param>
        /// <returns>A ValidationResult object.</returns>
        public static ValidationResult Validate(string email, string apikey, int timeout = 2000)
        {
            try
            {
                var response = GetClient(apikey, timeout).GetAsync(GetUri(email)).Result;
                EvaluateResponse(response);
                return JsonConvert.DeserializeObject<ValidationResult>(response.Content.ReadAsStringAsync().Result, _jsonSerializerSettings);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is TaskCanceledException)
                {
                    throw new TimeoutException("API call took longer than expected.");
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Validates an e-mail address in an asyncronous fashion.
        /// </summary>
        /// <param name="email">E-mail address to validate.</param>
        /// <param name="apikey">Mailgun e-mail validation API key.</param>
        /// <param name="timeout">Max. time to wait for the call to complete (in ms).</param>
        /// <returns>A Task containg a ValidationResult object.</returns>
        public static async Task<ValidationResult> ValidateAsync(string email, string apikey, int timeout = 2000)
        {
            try
            {
                var response = await GetClient(apikey, timeout).GetAsync(GetUri(email));
                EvaluateResponse(response);
                return JsonConvert.DeserializeObject<ValidationResult>(await response.Content.ReadAsStringAsync(), _jsonSerializerSettings);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is TaskCanceledException)
                {
                    throw new TimeoutException("API call took longer than expected.");
                }
                else
                {
                    throw;
                }
            }
        }

        private static void EvaluateResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Unexpected HTTP status code: {response.StatusCode}");
            }
        }

        private static HttpClient GetClient(string apikey, int timeout)
        {
            var auth = new HttpClientHandler()
            {
                Credentials = new NetworkCredential("api", apikey),
            };
            HttpClient client = new HttpClient(auth)
            {
                BaseAddress = new Uri(MailgunBaseUrl),
                Timeout = new TimeSpan(0, 0, 0, timeout),
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private static Uri GetUri(string email)
        {
            UriBuilder ub = new UriBuilder(MailgunBaseUrl)
            {
                Path = ValidatorResource,
                Query = $"address={email}",
            };
            return ub.Uri;
        }
    }
}
