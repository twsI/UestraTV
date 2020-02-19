using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using UestraTV.Data;

namespace UestraTV.Services
{
    public interface IUpdate
    {
        Task Update(DateTime currentTime, bool available, List<Broadcast> payload, string message);
    }

    public class UpdateHub : Hub<IUpdate>
    {
        public async Task SendUpdateToClients(DateTime currentTime, bool available, List<Broadcast> payload, string message)
        {
            await Clients.All.Update(currentTime, available, payload, message);
        }
    }
}
