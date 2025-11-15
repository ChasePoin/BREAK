using UnityEngine;

public class Ball : MonoBehaviour
{
    public float Size;
    public float Speed;
    public float GravityStrength;
    public float DirectionStrength;
    public float LargeSize = 5.00f;
    public float MediumSize = 2.50f;

    public float AdjustSpeed(float delta)
    {
        return Speed *= delta;
    }
    public float AdjustGravity(float delta)
    {
        return GravityStrength *= delta;
    }
    public float AdjustDirection(float delta)
    {
        return DirectionStrength *= delta;
    }
    public float AdjustSize(float delta)
    {
        return Size *= delta;
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
