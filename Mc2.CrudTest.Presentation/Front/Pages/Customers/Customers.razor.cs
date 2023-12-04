using Mc2.CrudTest.Presentation.Front.Services;
using Mc2.CrudTest.Shared.Helpers;
using Mc2.CrudTest.Shared.Models.Args;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using Mc2.CrudTest.Shared.Models;
using System.Linq;

namespace Mc2.CrudTest.Presentation.Front.Pages.Customers
{
    public partial class Customers:IDisposable
    {
        [Inject]
        public ICustomersService Service { get; set; }
        [Inject]
        public DialogAndNotificationService DialogAndNotificationService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected IEnumerable<CustomerDto> CustomersList;
        protected RadzenDataGrid<CustomerDto> Grid;
        protected IList<CustomerDto> selectedRow;
        private CancellationTokenSource cts = new();

        protected RadzenDataGridDataModel<CustomerDto> GridDataModel { get; set; }

        protected UpdateCustomer Modal { get; set; }
        protected CustomerDto Model { get; set; } = null;

        protected int TotalRow { get; set; } = 0;
        protected int Skip = 0;
        protected bool IsGridLoading { get; set; } = false;
        protected bool IsFirst { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            selectedRow = null;
            await GetData();
        }
        protected async void LoadData(LoadDataArgs args)
        {
            await GetData();
        }
        protected async Task GetData()
        {
            if (IsFirst)
            {
                IsFirst = false;
                return;
            }
            try
            {

                var filter = (Grid?.Query?.Filter) ?? "";
                var args = new LoadDataArgs
                {
                    Skip = (Grid?.Query?.Skip) ?? 0
                    ,
                    Top = (Grid?.Query?.Top) ?? 50
                    ,
                    OrderBy = Grid?.Query?.OrderBy
                    ,
                    Filter= Grid?.Query?.Filter
                };

                IsGridLoading = true;
                var response = await Service.Get(args, CancellationTokenHelper.GetCancellationToken(cts));
                if (response.IsSuccessful == true)
                {
                    CustomersList = response.Data?.List ?? new List<CustomerDto>();
                    Skip = args.Skip ?? 0;
                    TotalRow = response.Data?.TotalRow ?? 0;
                }
                else
                {
                    CustomersList = new List<CustomerDto>();
                    TotalRow = 0;
                    Skip = 0;
                    throw new Exception(response?.Errors.FirstOrDefault() ?? "unknown error");
                }
                IsGridLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                IsGridLoading = false;
                await DialogAndNotificationService.ErrorMessage(ex.Message);
                await InvokeAsync(StateHasChanged);
            }
        }
        protected async void Add_Click()
        {
            try
            {
                Model = new CustomerDto();
                Modal?.Show();
            }
            catch (Exception ex)
            {
                await DialogAndNotificationService.ErrorMessage(ex.Message);
            }
        }
        protected async void Edit_Click()
        {
            try
            {
                if (selectedRow != null && selectedRow.Count > 0)
                {
                    Model = selectedRow[0];
                    Modal?.Show();
                    selectedRow = null;
                }
                else
                {
                    await DialogAndNotificationService.WarningMessage("Please select a row");
                }
            }
            catch (Exception ex)
            {
                await DialogAndNotificationService.ErrorMessage(ex.Message);
            }
        }
        protected async void Delete_Click()
        {
            try
            {

                if (selectedRow != null && selectedRow.Count > 0)
                {
                    var deleteConfirmed = await DialogAndNotificationService.Confirm("Are you sure to delete country?", "Accept Delete");
                    if (deleteConfirmed)
                    {
                        var customer = selectedRow[0];
                        await Service.Delete(customer.Id, CancellationTokenHelper.GetCancellationToken(cts));
                        selectedRow = null;
                        await DialogAndNotificationService.SuccessMessage($"mr/mrs {customer.Firstname} {customer.Lastname} row removed successfully.");
                        await GetData();
                    }
                }
                else
                {
                    await DialogAndNotificationService.WarningMessage("Please select a row");
                }
            }
            catch (Exception ex)
            {
                await DialogAndNotificationService.ErrorMessage(ex.Message);
            }
        }
        public async void OnDialogClose(CustomerDto customer)
        {
            await GetData();
        }
        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
        }

    }
}
