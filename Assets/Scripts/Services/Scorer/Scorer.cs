using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


[CreateAssetMenu(fileName = "Scorer", menuName = "Services/Scorer")]

class Scorer : ScriptableObject
{
    public int Score { get; private set; }

    public Scorer()
    {
        ResetScore();
    } 

    public void Add(int score)
    {
        Score += 1;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}