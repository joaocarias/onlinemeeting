using Azure.Core;
using Azure.Identity;
using Joao.Lab.Api.IServices;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Joao.Lab.Api.Services
{
    public class GraphService : IGraphService
    {
        private string[] scopes = new[] { "https://graph.microsoft.com/.default" };

        // Multi-tenant apps can use "common",
        // single-tenant apps must use the tenant ID from the Azure portal
        private string tenantId = "fbe27a61-903e-4048-83a7-b7b6c43d367d";

        // Value from app registration
        private string clientId = "53a2b79b-60b6-4a1d-a8cf-62fbd9665918";

        // using Azure.Identity;
        private string clientSecret = "vHp8Q~GTAg11~v0u13gEbV9wph3EBCnZKGLoeaL4";

        private readonly ILogger<GraphService> _logger;

        public GraphService(ILogger<GraphService> logger)
        {
            _logger = logger;
        }

        public async Task<AccessToken> CreateAccessToken()
        {
            try
            {

                //Client Credentials -  Application
                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithTenantId(tenantId)
                    .WithClientSecret(clientSecret)
                    .Build();

                var authResult = await app
                       .AcquireTokenForClient(scopes)
                       .ExecuteAsync();

                var token = authResult.AccessToken;

                //// using Azure.Identity;
                //var options = new TokenCredentialOptions
                //{
                //    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                //};

                //// https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
                //var clientSecretCredential = new ClientSecretCredential(
                //    tenantId, clientId, clientSecret, options);

                //var tokenRequestContext = new TokenRequestContext(scopes);
                //var accessToken = await clientSecretCredential.GetTokenAsync(tokenRequestContext);

                //_logger.LogInformation("Token: " + accessToken.Token);

                return new AccessToken(token, DateTime.Now.AddDays(1));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return new AccessToken();
            }
        }

        public async Task<GraphServiceClient> CreateClient(AccessToken accessToken)
        {
            try
            {
                //  var token = "EwCAA8l6BAAUkj1NuJYtTVha+Mogk+HEiPbQo04AAUlNm4aaM4k0fmhHaRriK0gzwN7riSjcIggV08GWRm2eiEzVQN4Qt4T4az6gAKD4jIlxogagx42uHEx5MLLCfcTl+NkYwWRaiwkOUoTbhyJhJG9Bp0iMw5rRpV2iOMmZhMQBp+1HqrITn5QDKW55/68M0ckHw2cueafk7DgDSMDEY+01Mz0zJtDIRejsMED4jVNHJmE/GGi1p5OVbvJC2y/g54IzyRexYQ2M0CV+TFN1LWna9mvQmZ3Rykk6eHGx1jsjZBq4z53koo+dXqoTl0+q9ndlh2S6R/7SG5Rq24MAgMr6WgXirte9D6+iJLf/d2GXunK98rLIJaV94+m7cB4DZgAACCYVTK72C7SvUAIMYENfqbosj5XfBlSVy5oLOnaa6/r9N+WvrEvXat6yh3EkNa3RNCQ2bv0ZUHIRXFY76rHlN4dYGV/Ct4qt4XhV/1xz24epzXptNW8chCVNFXqIZQnEVgdE4AwxUySFAuJldnV3+rS0oBZfU/LGVvrz/n8EE7hGU4Ck+pOkXR/lcz++ivIJXQYTL1HzdiTzQo1olNcpfQhy5y+8sY/cb5mUdQfdnoleTCl0LN+Zt+Vg4d4XW/OTxUMwRPcMnygq2LXVOq/k6pyYRPaUmYVbsNa2NcSBMh+X9asFjitkmDb/z1SPsqvRUD/BSOWCmWsLIhJM0vs/zRjViQv6FdmZnmL141E9jx/S3LfEorXOmr0Hu42feHXcgAz1pFHRx9exUrEk+mewk6JgxG1xX+5aNUfp/wT/yifEJoQOO++2KkRvSU6aac6NeM1Mky4EeTf0HprB9zF3nK3sM/LTOUFBYtGGyF0Yp+HO/Z1gqiMe/t6bl5pw+CxBedm8BscHZmD+Io+HEFgOMyZyPL+gpftbK25qKm25zIMB4JB7r4T+DwxbHTu0Es0G2KOiw4eG2+YoGJKjXKEw4AiSdMez8abynCqYs2LAF4WgaDg2Q+rb/lI2eD8EIQNOiRXosoTYCP8QaseOF5jOY0IIdHcK3GmZHM3qi0/MVaPoKY8IhUDZ/GKFaZo3i3ysUuahXZK4O9IL7nJO0W9WXyqZpc6AyeRX/mt5kRaRQ85871gogIVtJsA9FqOPnjlugaWAKR0ZAht7WEA9beifvzAQszjN+UN3qIr5mgI=";
                var token = accessToken.Token;
                var graphClient = new GraphServiceClient(
                        new DelegateAuthenticationProvider(
                            async (request) =>
                            {
                                request.Headers.Authorization =
                                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                            }
                        )
                    );

                _logger.LogInformation("Graph Client created!");

                return graphClient;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return null;
            }
        }

       
    }
}
