using System;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var studentInfo = Console.ReadLine().Split();
            var studentFirstName = studentInfo[0];
            var studentSecondName = studentInfo[1];
            var studentFacultyNumber = studentInfo[2];

            var workerInfo = Console.ReadLine().Split();
            var workerFirstName = workerInfo[0];
            var workerSecondName = workerInfo[1];
            var workerWeekSalary = decimal.Parse(workerInfo[2]);
            var workerHoursPerDay = decimal.Parse(workerInfo[3]);


            var student = new Student(studentFirstName, studentSecondName, studentFacultyNumber);
            var worker = new Worker(workerFirstName, workerSecondName, workerWeekSalary, workerHoursPerDay);


            Console.WriteLine(Environment.NewLine + student);
            Console.WriteLine(Environment.NewLine + worker);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}