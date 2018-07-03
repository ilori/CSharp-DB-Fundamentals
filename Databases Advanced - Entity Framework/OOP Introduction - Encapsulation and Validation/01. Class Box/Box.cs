using System;

public class Box
{
    private double lenght;
    private double width;
    private double height;


    public Box(double lenght, double width, double height)
    {
        Lenght = lenght;
        Width = width;
        Height = height;
    }


    public double Lenght
    {
        get { return this.lenght; }
        set { this.lenght = value; }
    }

    public double Width
    {
        get { return this.width; }
        set { this.width = value; }
    }

    public double Height
    {
        get { return this.height; }
        set { this.height = value; }
    }

    public void SurfaceArea()
    {
        var surfaceArea = 2 * this.lenght * this.width + 2 * this.lenght * this.height +
                          2 * this.width * this.height;
        Console.WriteLine($"Surface Area - {surfaceArea:F2}");
    }

    public void LeteralArea()
    {
        var leteralArea = 2 * this.lenght * this.height + 2 * this.width * this.height;
        Console.WriteLine($"Lateral Surface Area - {leteralArea:f2}");
    }

    public void Volume()
    {
        var volume = this.lenght * this.width * this.height;
        Console.WriteLine($"Volume - {volume:f2}");
    }
}