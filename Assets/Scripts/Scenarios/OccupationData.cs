using System.Collections.Generic;

public class OccupationData
{
    public List<OccupationItem> Occupations { get; set; }
}

public class OccupationItem
{
    public string Occupation { get; set; }
    public List<string> Items { get; set; }
}
