using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


[CreateAssetMenu(fileName = "Scorer", menuName = "Services/Scorer")]

class Scorer : ScriptableObject
{
    public int passScore { get; private set; }

    public int rejectScore { get; private set; }

    public int dropScore { get; private set; }

    public Scorer()
    {
        ResetScores();
    } 

    public void Add(string type)
    {
        switch (type)
        {
            case "pass":
                passScore += 1;
                break;
            case "reject":
                rejectScore += 1;
                break;
            case "drop":
                dropScore += 1;
                break;
            default:
                break;
        }
    }

    public void ResetScores()
    {
        passScore = 0;
        rejectScore = 0;
        dropScore = 0;
    }
}