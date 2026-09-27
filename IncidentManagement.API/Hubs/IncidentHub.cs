using IncidentManagement.API.Data;
using IncidentManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;

namespace IncidentManagement.API.Hubs;

public class IncidentHub : Hub
{
    public const string DashboardGroup = "dashboard";

    private readonly IUserRepository _userRepository;

    public IncidentHub(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    //Dashboard viewers receive incident-wide events (created, status changed) for every incident.
    public Task JoinDashboard()
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, DashboardGroup);
    }

    public Task LeaveDashboard()
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, DashboardGroup);
    }

    //Join Incident
    public async Task JoinIncident(string incidentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"incident-{incidentId}");

        var user = await GetCurrentUserAsync();

        var onlineUser = new OnlineMemberDto
        {
            UserId = Context.UserIdentifier,
            FullName = user?.FullName,
            Email = user?.Email,
            ConnectionId = Context.ConnectionId
        };

        OnlineGroupMembers.AddMember($"incident-{incidentId}", onlineUser);

        await Clients.Users(Context.UserIdentifier!).SendAsync("OnlineMembers", OnlineGroupMembers.GetMembers($"incident-{incidentId}"));

        await Clients.Group($"incident-{incidentId}").SendAsync("UserJoined", onlineUser);
    }

    //Leave Incident
    public async Task LeaveIncident(string incidentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"incident-{incidentId}");

        var user = await GetCurrentUserAsync();

        var offlineUser = new OnlineMemberDto
        {
            UserId = Context.UserIdentifier,
            FullName = user?.FullName,
            Email = user?.Email,
            ConnectionId = Context.ConnectionId
        };

        OnlineGroupMembers.RemoveMember($"incident-{incidentId}", offlineUser.ConnectionId);

        await Clients.Group($"incident-{incidentId}").SendAsync("UserLeft", offlineUser);
    }

    public async Task Typing(string incidentId, bool isTyping)
    {
        var user = await GetCurrentUserAsync();

        await Clients.OthersInGroup($"incident-{incidentId}").SendAsync("UserTyping", new
        {
            userId = Context.UserIdentifier,
            fullName = user?.FullName,
            isTyping
        });
    }

    //Relay a chat message to everyone else in the room. Not persisted - the room is ephemeral.
    public async Task SendChatMessage(string incidentId, string messageId, string text)
    {
        var user = await GetCurrentUserAsync();

        await Clients.OthersInGroup($"incident-{incidentId}").SendAsync("ChatMessageReceived", new
        {
            id = messageId,
            text,
            authorId = Context.UserIdentifier,
            authorName = user?.FullName,
            sentAtUtc = DateTime.UtcNow
        });
    }

    //A recipient's client calls this as soon as it receives a message, confirming delivery back to the room.
    public Task AcknowledgeDelivery(string incidentId, string messageId)
    {
        return Clients.OthersInGroup($"incident-{incidentId}").SendAsync("MessageDelivered", new
        {
            messageId,
            userId = Context.UserIdentifier
        });
    }

    //A recipient's client calls this once the user has explicitly viewed the message(s).
    public Task AcknowledgeRead(string incidentId, IReadOnlyList<string> messageIds)
    {
        return Clients.OthersInGroup($"incident-{incidentId}").SendAsync("MessageRead", new
        {
            messageIds,
            userId = Context.UserIdentifier
        });
    }

    private Task<Domain.Entities.User?> GetCurrentUserAsync()
    {
        if (!Guid.TryParse(Context.UserIdentifier, out var userId))
        {
            return Task.FromResult<Domain.Entities.User?>(null);
        }

        return _userRepository.GetByIdAsync(userId, Context.ConnectionAborted);
    }
}
