public class Engine
{
    public int Speed { get; set; }
    public int Power { get; set; }

    public Engine(int speed,int power)
    {
        this.Power = power;
        this.Speed = speed;
    }
}