using UnityEngine;

public class WorldEventSpawner : MonoBehaviour
{
    public GameObject objToSpawn;
    public bool spawn;
    public int maxObj = 10;
    public int currentObjs;

    void Start()
    {
        currentObjs = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentObjs < maxObj)
        {
            float chance = 1f - ((float)currentObjs / maxObj);  // goes from 1 → 0

            if (Random.value < chance)
            {
                spawnObject();
                currentObjs += 1;
            }
        }
    }

   public void spawnObject()
    {
      Instantiate(objToSpawn, transform.position ,Quaternion.identity);
    }
}
