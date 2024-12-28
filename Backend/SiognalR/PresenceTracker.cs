using System;

namespace Backend.SiognalR;

public class PresenceTracker
{
    private static readonly Dictionary<string,List<string>> OnlineUsers= [];
    public Task<bool> userConnected(string username , string connectionId){
        var isOnline=false;
        lock (OnlineUsers)
        {
            if(OnlineUsers.ContainsKey(username))
            {
                OnlineUsers[username].Add(connectionId);
            }
            else{
                OnlineUsers.Add(username,[connectionId]);
                isOnline=true;
            }
        }
        return Task.FromResult(isOnline);
    }
    public Task<bool> UserDisconncected(string username , string connectionId)
    {
        var isOffline =false;
        lock(OnlineUsers){
            if(!OnlineUsers.ContainsKey(username))return Task.FromResult(isOffline);
            OnlineUsers[username].Remove(connectionId);
            if(OnlineUsers[username].Count==0)
            {
                OnlineUsers.Remove(username);
                isOffline=true;
            }
        }
        return Task.FromResult(isOffline);
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;
        lock(OnlineUsers){
            onlineUsers=OnlineUsers.OrderBy(k=>k.Key).Select(k=>k.Key).ToArray();
        }
        return Task.FromResult(onlineUsers);
    }
    public static Task<List<string>> GetConnectionsForUser(string username)
    {
        List<string> ConnectionIds;
        if(OnlineUsers.TryGetValue(username,out var connections))
        {
            lock(connections)
            {
                ConnectionIds= [.. connections];

            }
        }
        else ConnectionIds=[];
        return Task.FromResult(ConnectionIds);
    }
}
