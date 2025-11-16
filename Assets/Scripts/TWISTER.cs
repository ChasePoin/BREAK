using UnityEngine;

public class TWISTER : MonoBehaviour
{
    public float speed = 25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speed/2 * Time.deltaTime, speed * Time.deltaTime, speed/4 * Time.deltaTime); 
    }
}
