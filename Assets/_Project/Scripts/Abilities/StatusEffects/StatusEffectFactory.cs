public static class StatusEffectFactory
{
    public static IStatusEffect CreateStatusEffect(StatusEffectProperties properties)
    {
        switch (properties.Effect)
        {
            case StatusEffect.None:
                return null;
            case StatusEffect.Burn:
                return new BurnEffect(properties);
            default:
                return null;
        }
    }
}