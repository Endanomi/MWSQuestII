using System.Collections.Generic;

public class PersonProperties
{
    public string Name { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public string Occupation { get; set; }
    public List<string> Items { get; set; }
    public int MaxItemSize { get; set; }
    public string CorrectAction { get; set; }


    public PersonProperties(string[] values)
    {
        Name = values[0];
        Departure = values[1];
        Destination = values[2];
        Occupation = values[3];
        MaxItemSize = int.Parse(values[4]);
        Items = new List<string>(values[5].Split(", "));
        CorrectAction = values[6];
    }

    public PersonProperties(string name, string departure, string destination, string occupation, List<string> items, int maxItemSize, string correctAction)
    {
        Name = name;
        Departure = departure;
        Destination = destination;
        Occupation = occupation;
        Items = new List<string>(items);
        MaxItemSize = maxItemSize;
        CorrectAction = correctAction;
    }
}