using Mc2.CrudTest.Shared.Helpers;
using Mc2.CrudTest.Shared.Models.Args;
using Mc2.CrudTest.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Radzen;
using System.Threading;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace Mc2.CrudTest.Presentation.Front.Services
{
    public interface ICustomersService
    {
        Task<ResponseDTO<RadzenDataGridDataModel<CustomerDto>>> Get(LoadDataArgs args, CancellationToken? cancellationToken);
        Task<ResponseDTO<CustomerDto>> GetByID(int ID, CancellationToken? cancellationToken);
        Task<ResponseDTO<CustomerDto>> Insert(CustomerDto model, CancellationToken? cancellationToken);
        Task<ResponseDTO<CustomerDto>> Update(CustomerDto model, CancellationToken? cancellationToken);
        Task<VoidResponseDTO> Delete(int ID, CancellationToken? cancellationToken);
    }
    public class CustomersService : ICustomersService
    {
        private readonly  IHttpService _httpService;
        private readonly NavigationManager _navigationManager;

        private string _controllerName = "Customers";

        public CustomersService(IHttpService httpService, NavigationManager navigationManager)
        {
            _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
        }

        public async Task<VoidResponseDTO> Delete(int ID, CancellationToken? cancellationToken)
        {
            var result =await _httpService.Delete<VoidResponseDTO>($"/{_controllerName}/{ID}", cancellationToken);
            return result;
        }

        public async Task<ResponseDTO<RadzenDataGridDataModel<CustomerDto>>> Get(LoadDataArgs args, CancellationToken? cancellationToken)
        {
            var result = await _httpService.Post<ResponseDTO<RadzenDataGridDataModel<CustomerDto>>>($"/{_controllerName}/GetAll", args, cancellationToken);
            return result;
        }

        public async Task<ResponseDTO<CustomerDto>> GetByID(int ID, CancellationToken? cancellationToken)
        {
            var result = await _httpService.Get<ResponseDTO<CustomerDto>>($"/{_controllerName}/{ID}", cancellationToken);
            return result;
        }

        public async Task<ResponseDTO<CustomerDto>> Insert(CustomerDto model, CancellationToken? cancellationToken)
        {
            var result = await _httpService.Post<ResponseDTO<CustomerDto>>($"/{_controllerName}", model, cancellationToken);
            return result;
        }

        public async Task<ResponseDTO<CustomerDto>> Update(CustomerDto model, CancellationToken? cancellationToken)
        {
            var result = await _httpService.Put<ResponseDTO<CustomerDto>>($"/{_controllerName}", model, cancellationToken);
            return result;
        }

       
    }
}
