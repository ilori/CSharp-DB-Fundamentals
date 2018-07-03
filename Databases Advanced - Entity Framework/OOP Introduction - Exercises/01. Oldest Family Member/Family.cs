using System.Collections.Generic;
using System.Linq;

class Family
{
    private List<Person> people;

    public Family()
    {
        this.People = new List<Person>();
    }

    public List<Person> People
    {
        get { return this.people; }
        set { this.people = value; }
    }


    public void AddMember(Person person)
    {
        this.People.Add(person);
    }


    public Person GetOldestMember()
    {
        Person person = this.People.Aggregate((x, y) => x.Age >= y.Age ? x : y);

        return person;
    }
}