using UnityEngine;

public class DearthBox : MonoBehaviour
{
    // public Collider deathBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
    
    void OnTriggerEnter(Collider obj)
    {
        Destroy(obj);
        Debug.Log("obj removed by death box, saved from oblivion by oblion");
    }
}
