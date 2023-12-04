using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Threading;


namespace Mc2.CrudTest.Presentation.Front.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri, object value = null, CancellationToken? cancellationToken = null);
        Task Post(string uri, object value, CancellationToken? cancellationToken = null);
        Task<T> Post<T>(string uri, object value, CancellationToken? cancellationToken = null);
        Task Put(string uri, object value, CancellationToken? cancellationToken = null);
        Task<T> Put<T>(string uri, object value, CancellationToken? cancellationToken = null);
        Task Delete(string uri, CancellationToken? cancellationToken = null);
        Task<T> Delete<T>(string uri, CancellationToken? cancellationToken = null);
    }

    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager
        )
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public async Task<T> Get<T>(string uri, object value = null, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return default(T);
            }
            var request = createRequest(HttpMethod.Get, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Post(string uri, object value, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return;
            }
            var request = createRequest(HttpMethod.Post, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Post<T>(string uri, object value, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return default(T);
            }
            var request = createRequest(HttpMethod.Post, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Put(string uri, object value, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return;
            }
            var request = createRequest(HttpMethod.Put, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Put<T>(string uri, object value, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return default(T);
            }
            var request = createRequest(HttpMethod.Put, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Delete(string uri, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return;
            }
            var request = createRequest(HttpMethod.Delete, uri);
            await sendRequest(request);
        }

        public async Task<T> Delete<T>(string uri, CancellationToken? cancellationToken = null)
        {
            if (cancellationToken is null)
            {
                return default(T);
            }
            var request = createRequest(HttpMethod.Delete, uri);
            return await sendRequest<T>(request);
        }

        private HttpRequestMessage createRequest(HttpMethod method, string uri, object value = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (value != null)
                request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return request;
        }

        private async Task sendRequest(HttpRequestMessage request)
        {
            try
            {
                // send request
                using var response = await _httpClient.SendAsync(request);



                await handleErrors(response);
            }
            catch (TaskCanceledException ex)
            {
                return;
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
                return;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            try
            {
                // send request
                using var response = await _httpClient.SendAsync(request);


                await handleErrors(response);

                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                return await response.Content.ReadFromJsonAsync<T>(options);
            }
            catch (TaskCanceledException ex)
            {
                return default!;
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
                return default!;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task handleErrors(HttpResponseMessage response)
        {
            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<string>();
                throw new Exception(error);
            }
        }
    }
}
