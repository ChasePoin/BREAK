using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardSpawner : MonoBehaviour
{
    public GameObject pickupCardObject;
    public Collider capsule;
    public GameObject spawnedCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnedCard = Instantiate(pickupCardObject, transform.position, Quaternion.identity);
        capsule = transform.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate pickup card object? maybe put on that object itself
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("HELLO");
        PlayerController p_controller = player.gameObject.GetComponent<PlayerController>();
        if (p_controller == null)
        {
            Debug.Log("ERROR PLAYER CONTROLLER");
            return;
        }
        p_controller.GenerateRandomSingleCard();
        Destroy(spawnedCard);
        capsule.enabled = false;
        StartCoroutine(DelaySpawnNextCardDelay());
        
    }

    IEnumerator DelaySpawnNextCardDelay()
    {
        yield return new WaitForSeconds(10f);
        spawnedCard = Instantiate(pickupCardObject, transform.position, Quaternion.identity);
        capsule.enabled = true;
    }
        
}
