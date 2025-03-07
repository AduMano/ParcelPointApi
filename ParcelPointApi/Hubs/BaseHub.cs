using Microsoft.AspNetCore.SignalR;
using ParcelPointApi.Services;
using System.Security.Claims;

namespace ParcelPointApi.Hubs
{
    public class BaseHub : Hub
    {
        protected readonly UserConnectionManager _connectionManager;

        public BaseHub(UserConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var userIdToken = httpContext.Request.Headers["Authorization"].ToString();

            if (Guid.TryParse(userIdToken.Split(" ")[1], out Guid userId)) // Ensure it's a valid GUID
            {
                _connectionManager.AddConnection(userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _connectionManager.RemoveConnection(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
