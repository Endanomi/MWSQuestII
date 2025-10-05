using System.Collections.Generic;


public class LogRow : PersonProperties
{
    public string Action { get; private set; }

    public LogRow(string action, PersonProperties personProperties)
        : base(personProperties.Name, personProperties.Departure, personProperties.Destination, personProperties.Occupation, personProperties.Items, personProperties.MaxItemSize, personProperties.CorrectAction)
    {
        Action = action;
        if (action == "pass")
        {
            Action = "許可";
        }
        else if (action == "reject")
        {
            Action = "拒否";
        }
        else if (action == "drop")
        {
            Action = "逮捕";
        }
    }
}