using UnityEngine;

[RequireComponent(typeof(Health))]
public class GiveScoreOnDeath : MonoBehaviour
{
    [SerializeField] int scoreOnKill = 1;

    void Start()
    {
        var score = FindFirstObjectByType<ScoreManager>();
        var h = GetComponent<Health>();

        if (score == null || h == null) return;

        h.OnDeath += () => score.Add(scoreOnKill);
    }
}
