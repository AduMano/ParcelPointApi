using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ParcelPointApi.Services
{
    public class UserConnectionManager
    {
        private readonly ConcurrentDictionary<Guid, HashSet<string>> _userConnections = new();

        // Add a new connection for a user
        public void AddConnection(Guid userId, string connectionId)
        {
            if (!_userConnections.ContainsKey(userId))
                _userConnections[userId] = new HashSet<string>();

            _userConnections[userId].Add(connectionId);
        }

        // Remove a connection for a user
        public void RemoveConnection(string connectionId)
        {
            foreach (var user in _userConnections.Keys)
            {
                if (_userConnections[user].Remove(connectionId) && _userConnections[user].Count == 0)
                {
                    _userConnections.TryRemove(user, out _);
                    break;
                }
            }
        }

        // Get all connections for a user
        public HashSet<string> GetConnections(Guid userId)
        {
            Console.WriteLine(_userConnections);
            return _userConnections.TryGetValue(userId, out var connections) ? connections : new HashSet<string>();
        }

        
    }
}
