using UnityEngine;

public class Ball : MonoBehaviour
{
    public float Size;
    public float Speed;
    public float GravityStrength;
    public float DirectionStrength;
    public float LargeSize = 5.00f;
    public float MediumSize = 2.50f;

    public void AdjustSpeed(float delta)
    {
        Speed *= delta;
    }
    public void AdjustGravity(float delta)
    {
        GravityStrength *= delta;
    }
    public void AdjustDirection(float delta)
    {
        DirectionStrength *=  delta;
    }
    public string CheckSize(float size)
    {
        if (size >= LargeSize)
        {
            return "Large";
        }
        else if (size >= MediumSize && size <= LargeSize)
        {
            return "Medium";
        }
        else
        {
            return "Small";
        }
    }
}
