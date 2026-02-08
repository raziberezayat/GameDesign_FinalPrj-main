using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPointsRoot;

    void Start()
    {
        SpawnPlayerRandom();
    }

    void SpawnPlayerRandom()
    {
        if (playerPrefab == null || playerSpawnPointsRoot == null)
        {
            Debug.LogError("GameFlow: Assign playerPrefab and playerSpawnPointsRoot.");
            return;
        }

        int count = playerSpawnPointsRoot.childCount;
        if (count == 0)
        {
            Debug.LogError("GameFlow: No spawn points under PlayerSpawnPoints.");
            return;
        }

        int i = Random.Range(0, count);
        Transform sp = playerSpawnPointsRoot.GetChild(i);

        // Instantiate(playerPrefab, sp.position, Quaternion.identity);
        var p = Instantiate(playerPrefab, sp.position, Quaternion.identity);
Debug.Log("[GameFlow] Spawned: " + p.name);

var pc = p.GetComponent<PlayerCombat>();
if (pc == null) pc = p.AddComponent<PlayerCombat>();




    }
}
