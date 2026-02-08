using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }

    public void Add(int amount)
    {
        Score += amount;
        Debug.Log($"[Score] {Score}");
    }
}
