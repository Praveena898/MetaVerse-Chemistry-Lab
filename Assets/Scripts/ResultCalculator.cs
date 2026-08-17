using UnityEngine;

public class ResultCalculator : MonoBehaviour
{
    public static ResultCalculator Instance;

    public float finalMassOxalicAcid;

    void Awake()
    {
        Instance = this;
    }

    public void ComputeFinalResult(LabManager lab)
    {
        float N1 = lab.massOxalicAcidWeighed * 4f / 63f;

        float V1 = 25f;
        float N2 = (V1 * N1) / lab.volumeKMnO4Standardisation;

        float V4 = 25f;
        float N3 = (lab.volumeKMnO4Estimation * N2) / V4;

        float massPerLitre = N3 * 63f;
        finalMassOxalicAcid = massPerLitre / 4f;

        Debug.Log($"N1={N1}, N2={N2}, N3={N3}, Final mass={finalMassOxalicAcid} g");
    }
}