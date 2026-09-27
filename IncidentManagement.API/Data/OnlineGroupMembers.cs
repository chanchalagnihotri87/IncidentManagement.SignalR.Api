namespace IncidentManagement.API.Data
{
    public  class OnlineGroupMembers
    {
        private static readonly Dictionary<string, HashSet<OnlineMemberDto>> _groupMembers = new();

        public static void AddMember(string groupName,  OnlineMemberDto member)
        {
            lock (_groupMembers)
            {
                if (!_groupMembers.ContainsKey(groupName))
                {
                    _groupMembers[groupName] = new HashSet<OnlineMemberDto>();
                }
                _groupMembers[groupName].Add(member);
            }
        }
        public static void RemoveMember(string groupName, string userId)
        {
            lock (_groupMembers)
            {
                if (_groupMembers.ContainsKey(groupName))
                {
                    _groupMembers[groupName].RemoveWhere(m => m.UserId == userId);
                    if (_groupMembers[groupName].Count == 0)
                    {
                        _groupMembers.Remove(groupName);
                    }
                }
            }
        }
        public static IEnumerable<OnlineMemberDto> GetMembers(string groupName)
        {
            lock (_groupMembers)
            {
                if (_groupMembers.ContainsKey(groupName))
                {
                    return _groupMembers[groupName].ToList();
                }
                return Enumerable.Empty<OnlineMemberDto>();
            }
        }
    }
}


public record OnlineMemberDto {
   public string UserId { get; set;    }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string ConnectionId { get; set; }
}