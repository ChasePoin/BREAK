using UnityEngine;

public class WorldEventSpawner : MonoBehaviour
{
    public GameObject objToSpawn;
    public bool spawn;
    public int currentObjs;
    public int maxForThisObject;

    void Start()
    {
        currentObjs = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentObjs < GameManager.gm.maxBalls && currentObjs < maxForThisObject)
        {
            float chance = 1f / ((float)currentObjs * 30);

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
