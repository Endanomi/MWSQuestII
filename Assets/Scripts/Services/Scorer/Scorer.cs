using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


[CreateAssetMenu(fileName = "Scorer", menuName = "Services/Scorer")]

public class Scorer : ScriptableObject
{
    public int PassScore { get; private set; }

    public int RejectScore { get; private set; }

    public int DropScore { get; private set; }

    public int TotalCount = 0;

    public Scorer()
    {
        ResetScores();
    }

    public void Add(string type, PersonProperties properties)
    {
        TotalCount += 1;
        if (type.Equals(properties.CorrectAction))
        {
            switch (type)
            {
                case "pass":
                    PassScore += 1;
                    break;
                case "reject":
                    RejectScore += 1;
                    break;
                case "drop":
                    DropScore += 1;
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetScores()
    {
        PassScore = 0;
        RejectScore = 0;
        DropScore = 0;
    }

    public int TotalScore()
    {
        return PassScore + RejectScore + DropScore;
    }
}