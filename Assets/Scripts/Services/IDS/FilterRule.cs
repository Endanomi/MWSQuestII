using System.Collections.Generic;

namespace Services.IDS
{
    public class FilterRule
    {
        public string Action { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public string Occupation { get; set; }
        public string Item { get; set; }
        public int MaxItemSize { get; set; }

    }
}