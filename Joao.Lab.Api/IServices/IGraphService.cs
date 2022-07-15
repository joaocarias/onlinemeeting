using Azure.Core;
using Microsoft.Graph;

namespace Joao.Lab.Api.IServices
{
    public interface IGraphService
    {
        Task<GraphServiceClient> CreateClient(AccessToken accessToken);

        Task<AccessToken> CreateAccessToken();
    }
}
