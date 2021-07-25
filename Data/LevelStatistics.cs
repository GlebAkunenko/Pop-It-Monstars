using Firebase.Analytics;


public class LevelStatistics
{
    public string Name { get; set; }
    public int HpDelta { get; set; }
    public int Stars { get; set; }
    public int AdsHP { get; set; }
    public int AdsChest { get; set; }
    public int LoseHP { get; set; }
    public int AddHP { get; set; }

    public void SendDataToFirebase()
    {
        FirebaseAnalytics.LogEvent("MyEndLevel", new Parameter[] {
                new Parameter(FirebaseAnalytics.ParameterLevelName, MetaSceneDate.AnaliticsLevelName),
                new Parameter("hp_delta", HpDelta),
                new Parameter("stars", Stars),
                new Parameter("ads_to_hp", AdsHP),
                new Parameter("ads_to_chest", AdsChest)
            });

        FirebaseAnalytics.LogEvent("lose_hp_in_level", new Parameter(FirebaseAnalytics.ParameterValue, LoseHP));
        FirebaseAnalytics.LogEvent("add_hp_from_chest", new Parameter(FirebaseAnalytics.ParameterValue, AddHP));
        FirebaseAnalytics.LogEvent("add_hp_from_ads", new Parameter(FirebaseAnalytics.ParameterValue, AdsHP));
        FirebaseAnalytics.LogEvent("watch_ads_chest", new Parameter(FirebaseAnalytics.ParameterValue, AdsChest));
        FirebaseAnalytics.LogEvent("stars_in_level", new Parameter(FirebaseAnalytics.ParameterValue, Stars));
        FirebaseAnalytics.LogEvent("hp_delta", new Parameter(FirebaseAnalytics.ParameterValue, HpDelta));
    }
}
