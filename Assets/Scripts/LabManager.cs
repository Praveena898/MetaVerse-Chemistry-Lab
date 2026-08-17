using System;
using System.Collections.Generic;
using UnityEngine;

public class LabManager : MonoBehaviour
{
    public static LabManager Instance;

    public LabState CurrentState { get; private set; } = LabState.Intro;
    public event Action<LabState> OnStateChanged;

    public float massOxalicAcidWeighed;
    public float volumeKMnO4Standardisation;
    public float volumeKMnO4Estimation;

    public List<float> titresStandardisation = new List<float>();
    public List<float> titresEstimation = new List<float>();

    public LabState lastCheckpointState;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    #if UNITY_EDITOR
        RunTempTestSequence();
    #endif
    }

    #if UNITY_EDITOR
    void RunTempTestSequence()
    {
        // ----- Correct path (already confirmed working) -----
        RecordWeighedMass(0.78f);

        RecordTitreStandardisation(24.9f);
        RecordTitreStandardisation(25.0f);
        RecordTitreStandardisation(24.95f);

        RecordTitreEstimation(22.4f);
        RecordTitreEstimation(22.45f);
        RecordTitreEstimation(22.5f);

        // ----- Wrong-chemical validation tests -----
        Debug.Log("--- Validator tests ---");
        Debug.Log("NaOH (wrong reagent) -> " + ReactionValidator.CheckAcidification(ChemicalID.NaOH));
        Debug.Log("HCl (wrong reagent) -> " + ReactionValidator.CheckAcidification(ChemicalID.HCl));
        Debug.Log("Conc. H2SO4 (wrong concentration) -> " + ReactionValidator.CheckAcidification(ChemicalID.ConcH2SO4));
        Debug.Log("Dil. H2SO4 (correct) -> " + ReactionValidator.CheckAcidification(ChemicalID.DilH2SO4));
        Debug.Log("No heat applied -> " + ReactionValidator.CheckHeating(false));
        Debug.Log("Heat applied (correct) -> " + ReactionValidator.CheckHeating(true));
    }
    #endif

    public void AdvanceTo(LabState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
        Debug.Log($"Lab state -> {newState}");
    }
    public void SetCheckpoint()
    {
        lastCheckpointState = CurrentState;
        Debug.Log($"Checkpoint set at: {lastCheckpointState}");
    }
 
    public void RetryFromCheckpoint()
    {
        Debug.Log($"Retrying from checkpoint: {lastCheckpointState}");
        AdvanceTo(lastCheckpointState);
    }


    public void RecordWeighedMass(float mass)
    {
        massOxalicAcidWeighed = mass;
        AdvanceTo(LabState.Dissolving);
    }

    public void RecordTitreStandardisation(float volume)
    {
        titresStandardisation.Add(volume);

        if (titresStandardisation.Count >= 3)
        {
            volumeKMnO4Standardisation = ConcordanceChecker.GetConcordantValue(titresStandardisation);
            AdvanceTo(LabState.PipettingUnknown);
        }
        else
        {
            AdvanceTo(LabState.TitratingStandardisation);
        }
    }

    public void RecordTitreEstimation(float volume)
    {
        titresEstimation.Add(volume);

        if (titresEstimation.Count >= 3)
        {
            volumeKMnO4Estimation = ConcordanceChecker.GetConcordantValue(titresEstimation);
            AdvanceTo(LabState.Calculating);
            ResultCalculator.Instance.ComputeFinalResult(this);
        }
        else
        {
            AdvanceTo(LabState.TitratingEstimation);
        }
    }
}