﻿namespace DegreeClassEstimator.Model
{
    public interface IClassifier
    {
        void Classify(Degree degree);

        Result<ClassThresholds> GetThresholdSet(int availableCredit);
    }
}
