using Mc2.CrudTest.Presentation.Front.Services;
using Mc2.CrudTest.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Threading;
using Mc2.CrudTest.Shared.Models;
using System.Linq;
using System.Reflection;

namespace Mc2.CrudTest.Presentation.Front.Pages.Customers
{
    public partial class UpdateCustomer:IDisposable
    {
        [Inject]
        public ICustomersService Service { get; set; }
        [Inject]
        protected DialogAndNotificationService? DialogAndNotificationService { get; set; }
        [Parameter]
        public CustomerDto Model { get; set; } = new CustomerDto();
        [Parameter]
        public EventCallback<CustomerDto> CloseEventCallback { get; set; }

        private CancellationTokenSource cts = new();

        protected bool ShowModal { get; set; }

        bool IsProcessing = false;

        public async void Show()
        {
            ShowModal = true;
            await InvokeAsync(StateHasChanged);
        }
        protected void OnClose()
        {
            ShowModal = false;
            Model = null;
        }
        protected async Task HandleValidSubmit()
        {
            try
            {

                IsProcessing = true;
                ResponseDTO<CustomerDto> response;
                if (Model.Id!=0)
                {
                    response = await Service.Update(Model, CancellationTokenHelper.GetCancellationToken(cts));
                }
                else
                {
                    response = await Service.Insert(Model, CancellationTokenHelper.GetCancellationToken(cts));
                }
                if (response?.IsSuccessful==true)
                {
                    await DialogAndNotificationService.SuccessMessage(string.Format("Information changed successfully."));
                    IsProcessing = false;
                    ShowModal = false;
                    await CloseEventCallback.InvokeAsync(Model);
                }
                else
                {
                    throw new Exception(response?.Errors.FirstOrDefault() ?? "unknown error");
                }
            }
            catch (Exception ex)
            {
                IsProcessing = false;
                await DialogAndNotificationService.ErrorMessage(ex.Message);
            }
        }
        protected async void HandleInvalidSubmit()
        {
            await DialogAndNotificationService.WarningMessage(string.Format("Please enter all fields correctly"));
        }
        private async void StartDateValue_Change(DateTime? arg)
        {
            //   model.StartDateTime = arg;
        }
        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}
