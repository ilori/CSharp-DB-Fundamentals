using System;
using System.Globalization;

class DateModifier
{
    private string firstDate;
    private string secondDate;

    public DateModifier(string firstDate, string secondDate)
    {
        this.FirstDate = firstDate;
        this.SecondDate = secondDate;
    }

    public string FirstDate
    {
        get { return this.firstDate; }
        set { this.firstDate = value; }
    }

    public string SecondDate
    {
        get { return this.secondDate; }
        set { this.secondDate = value; }
    }


    public int CalculateDifference()
    {
        var firstDate = DateTime.ParseExact(this.FirstDate, "yyyy MM dd", CultureInfo.CurrentCulture);
        var secondDate = DateTime.ParseExact(this.SecondDate, "yyyy MM dd", CultureInfo.CurrentCulture);

        TimeSpan days = firstDate - secondDate;
        var dayDifference = (int) days.TotalDays;

        return dayDifference;
    }
}