using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        var n = int.Parse(Console.ReadLine());
        var songs = new List<Radio>();

        for (int i = 0; i < n; i++)
        {
            try
            {
                var tokens = Console.ReadLine().Split(';').ToList();

                var artistName = tokens[0];
                var songName = tokens[1];

                var songInformation = tokens[2].Split(':').ToList();
                int minutes;
                int seconds;
                if (songInformation.Count < 2 || !int.TryParse((songInformation[0]), out minutes) ||
                    !int.TryParse((songInformation[1]), out seconds))
                {
                    throw new ArgumentException("Invalid song length.");
                }
                var song = new Radio(artistName, songName, minutes, seconds);
                songs.Add(song);
                Console.WriteLine("Song added.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        var totalSeconds = songs.Select(x => x.Minutes).Sum() * 60 + songs.Select(x => x.Seconds).Sum();

        Console.WriteLine($"Songs added: {songs.Count}");
        Console.WriteLine($"Playlist length: {totalSeconds / 3600}h {totalSeconds / 60 % 60}m {totalSeconds % 60}s");
    }
}