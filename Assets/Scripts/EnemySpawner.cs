using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Shooter")]
    [SerializeField]
    GameObject shooterPrefab;
    [SerializeField]
    int maxShooterCount = 2;
    [SerializeField]
    Transform[] shooterSpawnLocations;
    
    

    [Header("Chaser")]
    [SerializeField]
    GameObject chaserPrefab;
    [SerializeField]
    int maxChaserCount = 2;
    [SerializeField]
    Transform[] chaserSpawnLocations;


    bool[] chaserSpawn;
    bool[] shooterSpawn;
    readonly System.Random rnd = new();

    void Start()
    {
        chaserSpawn = new bool[chaserSpawnLocations.Length];
        shooterSpawn = new bool[shooterSpawnLocations.Length];
    }
    void Update()
    {
        Populate(maxChaserCount, chaserSpawn, chaserPrefab, chaserSpawnLocations);
        Populate(maxShooterCount, shooterSpawn, shooterPrefab, shooterSpawnLocations);
    }

    private void Populate(int targetAmount, bool[] freeSpawns, GameObject prefab, Transform[] spawns)
    {
        int alive = GetBoolIndices(freeSpawns, true).Length;
        int missing = Mathf.Max(targetAmount - alive, 0);

        for (int i = 0; i < missing; i++)
        {
            int[] freeIndices = GetBoolIndices(freeSpawns, false);
            int freeIndex = freeIndices[rnd.Next(freeIndices.Length)];
            freeSpawns[freeIndex] = true;
            GameObject newShip = Instantiate(prefab, spawns[freeIndex].position, Quaternion.Euler(0, 0, (float)rnd.NextDouble() * 360));
            newShip.SendMessage("SetId", freeIndex);
        }
    }

    private int [] GetBoolIndices(bool [] list, bool value = true)
    {
        return list.Select((p, i) => new { Item = p, Index = i })
                           .Where(p => p.Item == value)
                           .Select(p => p.Index)
                           .ToArray();

    }

    public void SetSpawnFree(Tuple<string, int> arg)
    {
        if (arg.Item1 == "Chaser")
        {
            chaserSpawn[arg.Item2] = false;
        }

        else if (arg.Item1 == "Shooter")
        {
            shooterSpawn[arg.Item2] = false;
        }
    }
}
