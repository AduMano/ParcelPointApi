using Microsoft.AspNetCore.SignalR;
using ParcelPointApi.Data.Interface.Users;
using ParcelPointApi.Services;
using System;
using System.Threading.Tasks;

namespace ParcelPointApi.Hubs
{
    public class LockerHub : BaseHub
    {
        public LockerHub(UserConnectionManager connectionManager) : base(connectionManager)
        {
        }

        public class LockerStatusUpdateDTO
        {
            public Guid LockerId { get; set; }
            public string Size { get; set; }
            public bool IsOpen { get; set; }
            public Guid? UserOwner { get; set; }
            // Optionally, add other properties like UpdatedAt, Message, etc.
        }

        public async Task SendLockerStatusUpdate(Guid userID, LockerStatusUpdateDTO lockerUpdate)
        {
            var connections = _connectionManager.GetConnections(userID);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("LockerStatusUpdate", lockerUpdate);
            }
        }
    }
}
