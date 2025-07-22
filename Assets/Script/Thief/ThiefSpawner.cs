using System.Collections;
using UnityEngine;

public class ThiefSpawner : MonoBehaviour
{
    public GameObject[] thiefPrefab;
    public Transform[] spawnPoint;
    public Transform[] waypoints;

    public GameObject coinPrefab;

    [Range(0f, 1f)]
    public float spawnChance = 0.05f; // 5% 확률

    public float minSpawnInterval = 5f;  // 최소 간격
    public float maxSpawnInterval = 30f; // 최대 간격

    public AudioClip Dieclip;

    void Start()
    {
        StartCoroutine(ThiefSpawn());
    }

    private IEnumerator ThiefSpawn()
    {
        while (true)
        {

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            if(Random.value < spawnChance)
            {
              
                GameObject selectedPrefab = thiefPrefab[Random.Range(0, thiefPrefab.Length)];
                Transform selectedSpawn = spawnPoint[Random.Range(0, spawnPoint.Length)];

                GameObject thief = Instantiate(selectedPrefab, selectedSpawn.position, selectedSpawn.rotation);
                ThiefController controller = thief.GetComponent<ThiefController>();

                controller.waypoints = waypoints;
                controller.coinPrefab = coinPrefab;

                controller.Dieclip = Dieclip;

                Debug.Log("도둑 생성");
            }
        }
    }

}
