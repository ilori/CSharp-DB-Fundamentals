using System;

public class Radio
{
    private string songName;
    private string artistName;
    private int minutes;
    private int seconds;

    public Radio(string artistName, string songName, int minutes, int seconds)
    {
        this.ArtistName = artistName;
        this.SongName = songName;
        this.Minutes = minutes;
        this.Seconds = seconds;
    }

    private string ArtistName
    {
        set
        {
            if (value.Length < 3 || value.Length > 20)
            {
                throw new ArgumentException("Artist name should be between 3 and 20 symbols.");
            }

            this.artistName = value;
        }
    }

    private string SongName
    {
        set
        {
            if (value.Length < 3 || value.Length > 20)
            {
                throw new ArgumentException("Song name should be between 3 and 30 symbols.");
            }

            this.songName = value;
        }
    }

    public int Minutes
    {
        get { return this.minutes; }

        set
        {
            if (value < 0 || value > 14)
            {
                throw new ArgumentException("Song minutes should be between 0 and 14.");
            }

            this.minutes = value;
        }
    }

    public int Seconds
    {
        get { return this.seconds; }

        set
        {
            if (value < 0 || value > 59)
            {
                throw new ArgumentException("Song seconds should be between 0 and 59.");
            }
            this.seconds = value;
        }
    }
}