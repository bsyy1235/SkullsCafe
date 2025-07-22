using NUnit.Framework.Internal;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public float spawnInterval = 5f;

    private float timer;
    public Sprite[] itemIcons;
    private readonly string[] possibleItems = { "Coffee", "OrangeJuice", "Donut", "Cake" };
    public GameObject requestUIPrefab;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval)
        {
            SpawnCustomer();
            timer = 0f;
        }
    }

    void SpawnCustomer()
    {
        int index = Random.Range(0, customerPrefabs.Length);
        GameObject prefab = customerPrefabs[index];
        GameObject customer = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        string randomItem = possibleItems[Random.Range(0, possibleItems.Length)];

        CustomerInteraction interaction = customer.GetComponent<CustomerInteraction>();
        CustomerController controller = customer.GetComponent<CustomerController>();

        if (interaction != null)
        {
            interaction.requiredItem = randomItem;

            controller.requiredItem = randomItem;
            controller.itemIcons = itemIcons;
            controller.waypoints = waypoints;
            controller.requestUIPrefab = requestUIPrefab;
        }

        Debug.Log("Spawned customer wants: " + randomItem);
    }
}
