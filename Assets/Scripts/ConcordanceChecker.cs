using System.Collections.Generic;
using UnityEngine;

public static class ConcordanceChecker
{
    public static float GetConcordantValue(List<float> titres, float tolerance = 0.1f)
    {
        for (int i = 0; i < titres.Count; i++)
        {
            for (int j = i + 1; j < titres.Count; j++)
            {
                if (Mathf.Abs(titres[i] - titres[j]) <= tolerance)
                {
                    return (titres[i] + titres[j]) / 2f;
                }
            }
        }

        float sum = 0;
        foreach (var t in titres) sum += t;
        return sum / titres.Count;
    }
}