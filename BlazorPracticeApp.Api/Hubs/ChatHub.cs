namespace BlazorPracticeApp.Api.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public async Task JoinMovieChat(int movieId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"movie_{movieId}");
        }

        public async Task JoinPrivateChat(int userId1, int userId2)
        {
            string groupName = GetPrivateGroupName(userId1, userId2);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public static string GetPrivateGroupName(int userId1, int userId2)
        {
            int[] ids = new[] { userId1, userId2 };
            Array.Sort(ids);
            return $"private_{ids[0]}_{ids[1]}";
        }
    }
}
