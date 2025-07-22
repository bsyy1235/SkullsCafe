using System.Collections;
using UnityEngine;

public class ThiefSpawner : MonoBehaviour
{
    public GameObject[] thiefPrefab;
    public Transform[] spawnPoint;
    public Transform[] waypoints;

    public GameObject coinPrefab;

    [Range(0f, 1f)]
    public float spawnChance = 0.05f; // 5% Ȯ��

    public float minSpawnInterval = 5f;  // �ּ� ����
    public float maxSpawnInterval = 30f; // �ִ� ����

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

                Debug.Log("���� ����");
            }
        }
    }

}
