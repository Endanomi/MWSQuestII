using UnityEngine;

public class Visitor
{
    public string name;
    public string src;
    public string dst;
    public string job;
    public string size;
    public string item;

    public Visitor(string[] csvData)
    {
        name = csvData[0];
        src = csvData[1];
        dst = csvData[2];
        job = csvData[3];
        size = csvData[4];
        item = csvData[5];
    }
}
