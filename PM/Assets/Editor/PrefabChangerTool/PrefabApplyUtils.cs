namespace PM.PrefabChanger
{
    public static class PrefabApplyUtils
    {
        public static void ApplyPrefabConfigurationToCard(PrefabConfiguration config, Card card)
        {
            card.ApplyConfig(config);
        }
    }
}
