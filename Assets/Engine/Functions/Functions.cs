namespace enjoythevibes
{
    public static class Functions
    {
        public static float Lerp(float x1, float x2, float fx1, float fx2, float x)
        {
            var fx = fx1 + (x - x1) * ((fx2 - fx1)/(x2 - x1));
            return fx;
        }
    }
}