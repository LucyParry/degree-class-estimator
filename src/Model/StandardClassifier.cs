using System.Collections.Generic;
using System.Linq;

namespace DegreeClassEstimator.Model
{

    public class StandardClassifier : IClassifier
    {
        public ClassThresholds[] ThresholdsList { get; set; }

        public StandardClassifier(ClassThresholds[] thresholdsList)
        {
            ThresholdsList = thresholdsList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="degree"></param>
        public void Classify(Degree degree)
        {
            // Add counting modules for the required 120 at Level 3 first, then do the rest
            degree.GetCountingModules(degree.OrderedLevelThreeModules.ToList(), Constants.RequiredCreditsLevel3, true);
            degree.GetCountingModules(degree.UncountedModules.ToList(), Constants.RequiredCreditsAboveLevel1, false);
            degree.AddQualityAssuranceModules();

            Result<ClassThresholds> initialThresholdsResult;
            if (degree.CountingModuleCreditsForClassification < 1)
            {
                initialThresholdsResult = new Result<ClassThresholds>(false, new List<string> { "No un-transferred credit found - Unable to classify" });
            }
            else
            {
                initialThresholdsResult = this.GetThresholdSet(degree.CountingModuleCreditsForClassification);
                if (initialThresholdsResult.Success)
                    degree.InitialClassThresholds = initialThresholdsResult.ReturnObject;
            }

            if (!initialThresholdsResult.Success || degree.InitialClassThresholds is null)
            {
                foreach (string error in initialThresholdsResult?.Errors)
                {
                    degree.CalculationResult.Errors.Add(error);
                }
            }

            if (((int)degree.InitialClass == (int)degree.QualityAssuranceClass)
                || ((int)degree.InitialClass > (int)degree.QualityAssuranceClass))
            {
                degree.FinalClass = degree.InitialClass;
            }
            else
            {
                degree.FinalClass = degree.QualityAssuranceClass;
            }

            degree.IsCalculated = true;
            degree.CalculationResult = new Result<Degree>(success: true, returnObject: degree);
        }


        /// <summary>
        /// Get the set thresholds used for each degree class using the counting module Credits (excluding any transferred credit)
        /// </summary>
        /// <param name="availableCredit"></param>
        /// <returns></returns>
        public Result<ClassThresholds> GetThresholdSet(int availableCredit)
        {
            ClassThresholds thresholds = ThresholdsList.FirstOrDefault(x => x.AvailableCredit == availableCredit);
            if (thresholds is null)
            {
                return new Result<ClassThresholds>(false, new List<string> { "No threshold values found for Credits value" });
            }
            return new Result<ClassThresholds>(true, thresholds);
        }
    }
}
