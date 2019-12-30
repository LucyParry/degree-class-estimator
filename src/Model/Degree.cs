using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HonoursClassEstimator.Model
{
    /// <summary>
    /// Represents an Open University degree for the purposes of calculating its final classification
    /// </summary>
    public class Degree : IResult
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="thresholdsRepository"></param>
        public Degree()
        {
            IsCalculated = false;
            CalculationResult = new Result<Degree>(success: false, errors: new List<string> { });
            AllModules = new List<Module>();
            CountingModules = new List<Module>();
            QualityAssuranceModules = new List<Module>();
        }


        /// <summary>
        /// Whether the Degree class has been calculated yet
        /// </summary>
        public bool IsCalculated { get; set; }


        /// <summary>
        /// <see cref="Result"/> object containing the results of the Degree class calculation
        /// </summary>
        public Result<Degree> CalculationResult { get; set; }


        /// <summary>
        /// Each <see cref="Module"/> that has been associated with this <see cref="Degree"/> 
        /// </summary>
        public IList<Module> AllModules { get; set; }


        /// <summary>
        /// Each module that will count towards the <see cref="Degree"/>
        /// </summary>
        public IList<Module> CountingModules { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public int CurrentCountingModulePoints
        {
            get
            {
                var sum = this.CountingModules?.Select(x => x?.Points).Sum();
                return sum ?? 0;
            }
        }

        /// <summary>
        /// The <see cref="Module"/> objects that will be used in the 'Quality Assurance' calculation
        /// </summary>
        public IList<Module> QualityAssuranceModules { get; set; }

        public ClassThresholds InitialClassThresholds { get; set; }


        /// <summary>
        /// The sum of the points value of every <see cref="Module"/> associated 
        /// with the <see cref="Degree"/>
        /// </summary>
        public int AllModulePoints
        {
            get
            {
                var sum = this.AllModules?.Select(x => x?.Points).Sum();
                return sum ?? 0;
            }
        }

        /// <summary>
        /// The sum of the points value of each <see cref="Module"/> which has been 
        /// counted towards the <see cref="Degree"/> and is not transferred credit
        /// </summary>
        public int CountingModulePointsForClassification
        {
            get
            {
                var sum = this.CountingModules?
                    .Where(x => x.Grade != Grade.Transferred)
                    .Select(x => x?.Points).Sum();

                return Convert.ToInt32(sum);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public HonoursClass InitialClass
        {
            get
            {
                if ((!(InitialClassThresholds is null)) && DegreeWeightedPoints > 0)
                {
                    if (DegreeWeightedPoints <= InitialClassThresholds.FirstUpperBound) return HonoursClass.First;
                    if (DegreeWeightedPoints <= InitialClassThresholds.UpperSecondUpperBound) return HonoursClass.UpperSecond;
                    if (DegreeWeightedPoints <= InitialClassThresholds.LowerSecondUpperBound) return HonoursClass.LowerSecond;
                    if (DegreeWeightedPoints <= InitialClassThresholds.ThirdUpperBound) return HonoursClass.Third;
                }
                return HonoursClass.Unclassified;
            }
            private set { }
        }

        /// <summary>
        /// 
        /// </summary>
        public HonoursClass QualityAssuranceClass
        {
            get
            {
                if (QualityAssuranceWeightedPoints > 0)
                {
                    if (QualityAssuranceWeightedPoints <= Constants.QAClassThresholds.FirstUpperBound) return HonoursClass.First;
                    if (QualityAssuranceWeightedPoints <= Constants.QAClassThresholds.UpperSecondUpperBound) return HonoursClass.UpperSecond;
                    if (QualityAssuranceWeightedPoints <= Constants.QAClassThresholds.LowerSecondUpperBound) return HonoursClass.LowerSecond;
                    if (QualityAssuranceWeightedPoints <= Constants.QAClassThresholds.ThirdUpperBound) return HonoursClass.Third;
                }
                return HonoursClass.Unclassified;
            }
            private set { }
        }

        /// <summary>
        /// 
        /// </summary>
        public HonoursClass FinalClass { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        public void AddModule(Module module)
        {
            AllModules.Add(module);
            ResetClassifications();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        public void RemoveModule(Module module)
        {
            AllModules.Remove(module);
            ResetClassifications();
        }

        /// <summary>
        /// Set the thresholds used for calculating the initial class of the <see cref="Degree"/> 
        /// </summary>
        private Result<ClassThresholds> SetInitialClassThresholds(ClassThresholds[] thresholdsList)
        {
            if (this.CountingModulePointsForClassification < 1)
            {
                return new Result<ClassThresholds>(false,
                    new List<string> { "No un-transferred credit found - Unable to classify" });
            }
            else
            {
                Result<ClassThresholds> thresholdSetResult = 
                    GetThresholdSet((int)this.CountingModulePointsForClassification, thresholdsList);

                if (thresholdSetResult.Success)
                {
                    this.InitialClassThresholds = thresholdSetResult.ReturnObject;
                }

                return thresholdSetResult;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableCredit"></param>
        /// <returns></returns>
        public Result<ClassThresholds> GetThresholdSet(int availableCredit, ClassThresholds[] thresholdsList)
        {
            ClassThresholds thresholds = thresholdsList
                        .FirstOrDefault(x => x.AvailableCredit == availableCredit);

            if (thresholds is null)
            {
                return new Result<ClassThresholds>
                    (false, new List<string> { "No threshold values found for points value" });
            }
            else
            {
                return new Result<ClassThresholds>(true, thresholds);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void ResetClassifications()
        {
            IsCalculated = false;
            CalculationResult = new Result<Degree>(success: false, errors: new List<string> { });
            this.InitialClass = HonoursClass.Unclassified;
            this.QualityAssuranceClass = HonoursClass.Unclassified;
            this.FinalClass = HonoursClass.Unclassified;
            this.CountingModules = new List<Module>();
            this.QualityAssuranceModules = new List<Module>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int DegreeWeightedPoints
        {
            get
            {
                var sum = this.CountingModules?
                    .Where(x => x.Grade != Grade.Transferred)
                    .Select(x => x?.FinalWeightedPoints).Sum();

                return sum ?? 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int QualityAssuranceWeightedPoints
        {
            get
            {
                var sum = this.QualityAssuranceModules?
                    .Where(x => x.Grade != Grade.Transferred)
                    .Select(x => x?.GradeWeightedPoints).Sum();
                return sum ?? 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Module> OrderedLevelThreeModules
        {
            get
            {
                return AllModules
                    .Where(x => x.Level == Level.Three)
                    .OrderBy(x => (int)x.Grade)
                    .OrderByDescending(x => x.Compulsary ? 1 : 0)
                    .ToList();
            }
        }

        public List<Module> OrderedCountedLevelThreeModules
        {
            get
            {
                return CountingModules
                    .Where(x => x.Level == Level.Three)
                    .OrderBy(x => (int)x.Grade)
                    .ToList();
            }
        }

        public List<Module> UncountedModules
        {
            get
            {
                return AllModules
                    .Where(x => !CountingModules.Contains(x))
                    .OrderBy(x => (int)x.Grade)
                    .OrderByDescending(x => x.Compulsary ? 1 : 0)
                    .ToList();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Validate()
        {
            string generalInstruction = "Refer to the instructions for the minimum requirements for calculating your degree class";

            if (this.AllModulePoints < Constants.RequiredPointsAboveLevel1)
            {
                return $"Not enough total module credits - {generalInstruction}";
            }
            if (this.AllModules?.Where(x => x?.Level == Level.Three).Select(x => x?.Points).Sum() < Constants.RequiredPointsLevel3)
            {
                return $"Not enough Level 3 credits - {generalInstruction}";
                
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Classify(ClassThresholds[] thresholdsList)
        {
            ResetClassifications();

            string validationResult = Validate();
            if (!(validationResult == ""))
            {
                CalculationResult.Errors.Add(validationResult);
            }

            GetCountingModules(OrderedLevelThreeModules, Constants.RequiredPointsLevel3, true);
            GetCountingModules(UncountedModules, Constants.RequiredPointsAboveLevel1, false);
            AddQualityAssuranceModules();

            Result<ClassThresholds> initialThresholdsResult = SetInitialClassThresholds(thresholdsList);
            if (!initialThresholdsResult.Success || this.InitialClassThresholds is null)
            {
                foreach (string error in initialThresholdsResult?.Errors)
                {
                    CalculationResult.Errors.Add(error);
                }
            }

            if (((int)this.InitialClass == (int)this.QualityAssuranceClass)
                || ((int)this.InitialClass > (int)this.QualityAssuranceClass))
            {
                this.FinalClass = this.InitialClass;
                this.IsCalculated = true;
            }
            else
            {
                this.FinalClass = this.QualityAssuranceClass;
                this.IsCalculated = true;
            }

            CalculationResult = new Result<Degree>(success: true, returnObject: this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableModules"></param>
        /// <param name="maxPoints"></param>
        /// <param name="doubleWeight"></param>
        private void GetCountingModules(List<Module> availableModules, int maxPoints, bool doubleWeight)
        {
            foreach (var nextModule in availableModules)
            {
                if (CurrentCountingModulePoints < maxPoints)
                {
                    var nextModuleInitialPoints = nextModule.Points;
                    if (nextModule.Points + CurrentCountingModulePoints > maxPoints)
                    {
                        nextModule.Description += " (Partial)";
                        nextModule.Points = (maxPoints - CurrentCountingModulePoints);
                        AllModules.Add(new Module()
                        {
                            Description = nextModule.Description,
                            Points = nextModuleInitialPoints - nextModule.Points,
                            Level = nextModule.Level,
                            Compulsary = nextModule.Compulsary,
                            Grade = nextModule.Grade,
                        });
                    }
                    nextModule.DoubleWeight = doubleWeight;
                    this.CountingModules.Add(nextModule);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddQualityAssuranceModules()
        {
            var qaPoints = 0;
            foreach (var module in this.OrderedCountedLevelThreeModules)
            {
                if (qaPoints < Constants.RequiredQualityAssurancePoints)
                {
                    if ((module.Points + qaPoints) > Constants.RequiredQualityAssurancePoints)
                    {
                        this.QualityAssuranceModules.Add(new Module()
                        {
                            Points = module.Points - (Constants.RequiredQualityAssurancePoints - qaPoints),
                            Level = Level.Three,
                            Grade = module.Grade
                        });
                    }
                    else
                    {
                        this.QualityAssuranceModules.Add(module);
                    }
                    qaPoints += module.Points;
                }
            }
        }

        public string GetModuleCodeString()
        {
            string[] moduleCodes = AllModules.Select(x => x.ModuleDescriptionCode).ToArray();
            return string.Join("&", moduleCodes);
        }

        /// <summary>
        /// Return a list of <see cref="Module"/> based on any <see cref="ModuleDescriptionCode"/> found in the <paramref name="degreeString"/>
        /// </summary>
        public static List<Module> GetModules(string degreeString)
        {
            var modules = new List<Module>();
            MatchCollection matches = Regex.Matches(degreeString, Module.ModuleDescriptionCodePattern);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    modules.Add(Module.GetModule(match.Value));
                }
                return modules;
            }
            else
            {
                return null;
            }
        }

    }    
}