namespace DegreeClassEstimator.Model
{
    public static class Constants
    {
        public static int ModuleMaxCredits => 360;
        public static int RequiredCreditsAboveLevel1 => 240;
        public static int RequiredCreditsLevel3 => 120;
        public static int RequiredQualityAssuranceCredits => 60;


        public static ClassThresholds QAClassThresholds => new()
        {
            FirstUpperBound = 60,
            UpperSecondUpperBound = 120,
            LowerSecondUpperBound = 180,
            ThirdUpperBound = 240
        };
    }
}
