using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Objects to Spawn")]
    public GameObject objectToSpawn1; // Primeiro objeto
    public GameObject objectToSpawn2; // Segundo objeto
    public GameObject objectToSpawn3; // Terceiro objeto

    [Header("Spawn Points")]
    public Transform spawnPoint1;    // Ponto de spawn do primeiro objeto
    public Transform spawnPoint2;    // Ponto de spawn do segundo objeto
    public Transform spawnPoint3;    // Ponto de spawn do terceiro objeto

    // Função para spawnar o primeiro objeto
    public void SpawnObject1()
    {
        if (objectToSpawn1 != null && spawnPoint1 != null)
        {
            Instantiate(objectToSpawn1, spawnPoint1.position, spawnPoint1.rotation);
        }
    }

    // Função para spawnar o segundo objeto
    public void SpawnObject2()
    {
        if (objectToSpawn2 != null && spawnPoint2 != null)
        {
            Instantiate(objectToSpawn2, spawnPoint2.position, spawnPoint2.rotation);
        }
    }

    // Função para spawnar o terceiro objeto
    public void SpawnObject3()
    {
        if (objectToSpawn3 != null && spawnPoint3 != null)
        {
            Instantiate(objectToSpawn3, spawnPoint3.position, spawnPoint3.rotation);
        }
    }
}
