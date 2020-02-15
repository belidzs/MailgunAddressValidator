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
        private const string MailgunValidatorResource = "/v3/address/validate";
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            },
        };

        /// <summary>
        /// Validates an e-mail address.
        /// </summary>
        /// <param name="email">E-mail address to validate.</param>
        /// <param name="apikey">Mailgun e-mail validation API key.</param>
        /// <param name="timeout">Max. time to wait for the call to complete (in ms).</param>
        /// <returns>A ValidationResult object.</returns>
        public static ValidationResult Validate(string email, string apikey, int timeout = 2000)
        {
            try
            {
                var response = AsyncHelper.RunSync(() => GetConfiguredClient(apikey, timeout).GetAsync(GetUri(email)));
                EvaluateResponse(response);
                string responseBody = AsyncHelper.RunSync(() => response.Content.ReadAsStringAsync());
                return JsonConvert.DeserializeObject<ValidationResult>(responseBody, _jsonSerializerSettings);
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException("API call took longer than expected.");
            }
        }

        /// <summary>
        /// Validates an e-mail address.
        /// </summary>
        /// <param name="email">E-mail address to validate.</param>
        /// <param name="apikey">Mailgun e-mail validation API key.</param>
        /// <param name="timeout">Max. time to wait for the call to complete (in ms).</param>
        /// <returns>A Task containg a ValidationResult object.</returns>
        public static async Task<ValidationResult> ValidateAsync(string email, string apikey, int timeout = 2000)
        {
            try
            {
                var response = await GetConfiguredClient(apikey, timeout).GetAsync(GetUri(email)).ConfigureAwait(false);
                EvaluateResponse(response);
                string responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<ValidationResult>(responseBody, _jsonSerializerSettings);
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException("API call took longer than expected.");
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

        private static HttpClient GetConfiguredClient(string apikey, int timeout)
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
                Path = MailgunValidatorResource,
                Query = $"address={email}",
            };
            return ub.Uri;
        }
    }
}
