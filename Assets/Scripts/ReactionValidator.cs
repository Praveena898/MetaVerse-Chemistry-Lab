public static class ReactionValidator
{
    public static ValidationResult CheckAcidification(ChemicalID acidUsed)
    {
        if (acidUsed == ChemicalID.DilH2SO4) return ValidationResult.Correct;
        if (acidUsed == ChemicalID.ConcH2SO4) return ValidationResult.WrongConcentration;
        return ValidationResult.WrongReagent;
    }

    public static ValidationResult CheckHeating(bool wasHeated)
    {
        return wasHeated ? ValidationResult.Correct : ValidationResult.MissingHeat;
    }
}