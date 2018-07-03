using System;
using System.Collections.Generic;
using System.Linq;

public class Team
{
    private string name;
    private List<Player> players;

    public Team(string name)
    {
        this.Name = name;
        this.players = new List<Player>();
    }

    public string Name
    {
        get { return this.name; }
        private set
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("A name should not be empty.");
            }
            this.name = value;
        }
    }

    public IReadOnlyList<Player> Players
    {
        get { return this.players.AsReadOnly(); }
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public void RemovePlayer(string playerName)
    {
        bool containsPlayer = this.players.Any(p => p.Name == playerName);
        if (!containsPlayer)
        {
            throw new ArgumentException(string.Format($"Player {playerName} is not in {this.Name} team."));
        }
        var player = this.players.First(p => p.Name == playerName);
        this.players.Remove(player);
    }
}