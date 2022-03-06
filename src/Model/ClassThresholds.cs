namespace DegreeClassEstimator.Model
{
    public class ClassThresholds : IResult
    {
        public int AvailableCredit { get; set; }

        public decimal FirstUpperBound { get; set; }

        public decimal UpperSecondUpperBound { get; set; }

        public decimal LowerSecondUpperBound { get; set; }

        public decimal ThirdUpperBound { get; set; }
    }
}
