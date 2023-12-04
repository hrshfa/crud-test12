using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Mc2.CrudTest.Presentation.Front
{
    public class AntiforgeryHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-CSRF", "1");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
