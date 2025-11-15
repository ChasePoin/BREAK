using UnityEngine;

public class WorldEventSpawner : MonoBehaviour
{
    public GameObject objToSpawn;
    public bool spawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn)
        {
            spawnObject();
        }
        
    }

   public void spawnObject()
    {
      Instantiate(objToSpawn, transform.position ,Quaternion.identity);
    }
}
