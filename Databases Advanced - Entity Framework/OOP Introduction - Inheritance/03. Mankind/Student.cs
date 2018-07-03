using System;
using ConsoleApplication;

public class Student : Human
{
    private string facultyNumber;

    public Student(string firstName, string lastName, string facultyNumber) : base(firstName, lastName)
    {
        this.FacultyNumber = facultyNumber;
    }


    public string FacultyNumber
    {
        get { return this.facultyNumber; }
        set
        {
            if (value.Length < 5 || value.Length > 10)
            {
                throw new ArgumentException("Invalid faculty number!");
            }
            foreach (var c in value)
            {
                if (!char.IsLetter(c) && !char.IsDigit(c))
                {
                    throw new ArgumentException("Invalid faculty number!");
                }
            }
            this.facultyNumber = value;
        }
    }


    public override string ToString()
    {
        return $"First Name: {this.FirstName}\nLast Name: {this.LastName}\nFaculty number: {this.FacultyNumber}";
    }
}