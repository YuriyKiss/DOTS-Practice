using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    private Collider col;

    public int amount;
    public GameObject prefab;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < amount; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-col.bounds.extents.x, col.bounds.extents.x),
                    Random.Range(-col.bounds.extents.y, col.bounds.extents.y),
                    Random.Range(-col.bounds.extents.z, col.bounds.extents.z)
                    );
                Instantiate(prefab, gameObject.transform.position + randomPosition, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
