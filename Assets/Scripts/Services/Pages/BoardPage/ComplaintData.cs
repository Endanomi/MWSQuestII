using System;  
using System.Collections.Generic;

[Serializable]
public class ComplaintItem
{
    public string name;
    public string text;
    public string detail; // �ǉ�
}

[Serializable]
public class ComplaintData
{
    public List<ComplaintItem> complaints;
}
