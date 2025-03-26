using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AdilBooks.Hubs
{
    public class VoteHub : Hub
    {
        public async Task SendVoteUpdate(int showId)
        {
            await Clients.All.SendAsync("ReceiveVoteUpdate", showId);
        }
    }
}
