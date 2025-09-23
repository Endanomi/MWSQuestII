using System;  
using System.Collections.Generic;

[Serializable]
public class ComplaintItem
{
    public string name;
    public string text;
    public string detail; // ’Ç‰Á
}

[Serializable]
public class ComplaintData
{
    public List<ComplaintItem> complaints;
}
