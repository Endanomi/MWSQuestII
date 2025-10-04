using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;


public class PropertiesCreator
{
    private OccupationData occupationData;
    private LocationData locationData;
    public PropertiesCreator()
    {
        TextAsset occJsonFile = Resources.Load<TextAsset>($"Spawn/Occupations");
        string occJson = occJsonFile.text;
        TextAsset locationsJsonFile = Resources.Load<TextAsset>($"Spawn/Locations");
        string locationsJson = locationsJsonFile.text;

        // デシリアライズ
        occupationData = JsonConvert.DeserializeObject<OccupationData>(occJson);
        locationData = JsonConvert.DeserializeObject<LocationData>(locationsJson);
    }




    public PersonProperties CreatePersonProperties()
    {

        // ランダムにoccJsonから職業を選ぶ
        System.Random random = new System.Random();
        int randomOccupationIndex = random.Next(occupationData.Occupations.Count);
        
        // itemを1個にするか、2個にするかランダムに決める
        int randomItemCount = random.Next(1, 3);
        List<string> items = new List<string>();

        // 被りがないようにitemを選ぶ
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < randomItemCount)
        {
            int randomItemIndex = random.Next(occupationData.Occupations[randomOccupationIndex].Items.Count);
            selectedIndices.Add(randomItemIndex);
        }

        foreach (int index in selectedIndices)
        {
            items.Add(occupationData.Occupations[randomOccupationIndex].Items[index]);
        }

        // DepartureとDestinationをランダムに選ぶ
        int randomDepartureIndex = random.Next(locationData.outer.Count);
        int randomDestinationIndex = random.Next(locationData.inner.Count);

        // 1〜100の間でランダムな整数を生成
        int randomSizeValue = random.Next(1, 101);

        return new PersonProperties
        {
            Departure = locationData.outer[randomDepartureIndex],
            Destination = locationData.inner[randomDestinationIndex],
            Occupation = occupationData.Occupations[0].Occupation,
            Items = items,
            MaxItemSize = randomSizeValue
        };
    }
}