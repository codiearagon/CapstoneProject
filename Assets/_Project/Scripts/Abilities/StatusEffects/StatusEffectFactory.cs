public static class StatusEffectFactory
{
    public static IStatusEffect CreateStatusEffect(StatusEffectProperties properties)
    {
        switch (properties.Effect)
        {
            case StatusEffect.None:
                return null;
            case StatusEffect.Dot:
                return new DotEffect(properties);
            case StatusEffect.Slow:
                return new SlowEffect(properties);
            default:
                return null;
        }
    }
}