using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        string input;
        var teams = new List<Team>();
        while ((input = Console.ReadLine()) != "END")
        {
            string[] tokens = input.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            switch (tokens[0])
            {
                case "Team":
                    teams.Add(new Team(tokens[1]));
                    break;
                case "Add":
                    var teamName = tokens[1];
                    var playerName = tokens[2];
                    var endurance = int.Parse(tokens[3]);
                    var sprint = int.Parse(tokens[4]);
                    var dribble = int.Parse(tokens[5]);
                    var passing = int.Parse(tokens[6]);
                    var shooting = int.Parse(tokens[7]);

                    var doesTeamExist = teams.Any(t => t.Name == teamName);
                    Team team;
                    if (!doesTeamExist)
                    {
                        Console.WriteLine($"Team {teamName} does not exist.");
                    }
                    else
                    {
                        try
                        {
                            team = teams.First(t => t.Name == teamName);
                            var player = new Player(playerName, endurance, sprint, dribble, passing, shooting);
                            team.AddPlayer(player);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    break;
                case "Remove":
                    teamName = tokens[1];
                    team = teams.First(t => t.Name == teamName);
                    playerName = tokens[2];
                    try
                    {
                        team.RemovePlayer(playerName);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    break;
                case "Rating":
                    teamName = tokens[1];
                    doesTeamExist = teams.Any(t => t.Name == teamName);
                    if (!doesTeamExist)
                    {
                        Console.WriteLine($"Team {teamName} does not exist.");
                    }
                    else
                    {
                        team = teams.First(t => t.Name == teamName);
                        var totalSkill = 0;
                        foreach (var player in team.Players)
                        {
                            totalSkill += player.AverageRating;
                        }
                        Console.WriteLine($"{teamName} - {totalSkill}");
                    }
                    break;
            }
        }
    }
}