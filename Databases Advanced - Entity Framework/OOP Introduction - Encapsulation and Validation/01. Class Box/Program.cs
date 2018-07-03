using System;
using System.Linq;
using System.Reflection;

public class Program
{
    public static void Main()
    {
        Type boxType = typeof(Box);
        FieldInfo[] fields = boxType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine(fields.Count());
        
        var box = new Box(double.Parse(Console.ReadLine()),double.Parse(Console.ReadLine()),double.Parse(Console.ReadLine()));
        box.SurfaceArea();
        box.LeteralArea();
        box.Volume();
    }
}