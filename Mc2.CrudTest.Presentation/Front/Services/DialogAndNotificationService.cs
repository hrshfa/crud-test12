using CurrieTechnologies.Razor.SweetAlert2;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Front.Services
{
    public class DialogAndNotificationService
    {

        private SweetAlertService Swal;

        public DialogAndNotificationService(SweetAlertService service)
        {
            Swal = service;
        }
        public async Task<bool> Confirm(string? message, string title = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                // async/await
                SweetAlertResult result = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = title != "" ? title : null,
                    Text = message,
                    Icon = SweetAlertIcon.Question,
                    ShowCancelButton = true,
                    ConfirmButtonText = "Yes",
                    CancelButtonText = "No"
                });
                return result.IsConfirmed;
            }
            else
            {
                return false;
            }
        }
        public async Task SuccessMessage(string? message, string title = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = title != "" ? title : null,
                    Text = message,
                    Position = SweetAlertPosition.BottomEnd,
                    Icon = SweetAlertIcon.Success,
                    Timer = 4000
                }
              );
            }
        }
        public async Task ErrorMessage(string? message, string title = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = title != "" ? title : null,
                    Text = message,
                    Icon = SweetAlertIcon.Error,
                    Position = SweetAlertPosition.BottomEnd,
                    Timer = 4000
                }
              );
            }

        }
        public async Task WarningMessage(string? message, string title = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = title != "" ? title : null,
                    Text = message,
                    Icon = SweetAlertIcon.Warning,
                    ShowConfirmButton = false,
                    Position = SweetAlertPosition.BottomEnd,
                    Timer = 4000
                }
              );
            }
        }
    }
}
