namespace HonoursClassEstimator.Model
{
    public static class Constants
    {
        public static int ModuleMaxPoints => 360;
        public static int RequiredPointsAboveLevel1 => 240;
        public static int RequiredPointsLevel3 => 120;
        public static int RequiredQualityAssurancePoints => 60;


        public static ClassThresholds QAClassThresholds => new ClassThresholds()
        {
            FirstUpperBound = 60,
            UpperSecondUpperBound = 120,
            LowerSecondUpperBound = 180,
            ThirdUpperBound = 240
        };
    }
}
