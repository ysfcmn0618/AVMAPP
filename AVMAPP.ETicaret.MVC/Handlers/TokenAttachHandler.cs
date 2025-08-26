using System.Net.Http.Headers;

namespace AVMAPP.ETicaret.MVC.Handlers
{
    public class TokenAttachHandler(IHttpContextAccessor _httpContextAccessor):DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;
            var token = context?.Request.Cookies["jwt_token"];

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
