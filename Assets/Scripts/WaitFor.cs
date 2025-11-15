using System.Collections;
    public static class WaitFor
    {
        public static IEnumerator Frames(int frameCount)
        {
            if (frameCount < 1)
            {
                throw new System.ArgumentOutOfRangeException("frameCount", "Must be greater than 0.");
            }
            while (frameCount > 0)
            {
                frameCount--;
                yield return null; // Yields for one frame
            }
        }
    }