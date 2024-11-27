using UnityEngine;

public class SpawnAndDestroy : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject objectToSpawn; // Prefab do GameObject a ser instanciado
    public Transform spawnPoint;    // Local onde o objeto será instanciado

    [Header("Lifetime Settings")]
    public float destroyAfterSeconds = 2f; // Tempo para destruir o objeto

    public void SpawnObjectVFX()
    {
        if (objectToSpawn != null && spawnPoint != null)
        {
            // Instancia o objeto no local definido
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);

            // Destroi o objeto após o tempo especificado
            Destroy(spawnedObject, destroyAfterSeconds);
        }
    }
}
