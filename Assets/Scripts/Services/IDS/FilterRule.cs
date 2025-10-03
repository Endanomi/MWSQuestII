using System.Collections.Generic;

namespace Services.IDS
{
    public class FilterRule
    {
        public string Action { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Occupation { get; set; }
        public string Item { get; set; }
        public string MaxItemSize { get; set; }

    }
}