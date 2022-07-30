using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DegreeClassEstimator.Model
{
    /// <summary>
    /// Represents an Open University undergraduate honours degree for the purposes of calculating its final classification
    /// </summary>
    public class Degree
    {
        public Degree()
        {
            CalculationResult = new Result<Degree>(success: false, errors: new List<string> { });
            AllModules = new List<Module>();
            CountingModules = new List<Module>();
            QualityAssuranceModules = new List<Module>();
        }


        public bool IsCalculated { get; set; }


        /// <summary>
        /// A <see cref="Result"/> object containing the results of the Degree class calculation
        /// </summary>
        public Result<Degree> CalculationResult { get; set; }

        /// <summary>
        /// Each <see cref="Module"/> that has been associated with this <see cref="Degree"/> 
        /// </summary>
        public IList<Module> AllModules { get; set; }

        /// <summary>
        /// The sum of the Credits value of every <see cref="Module"/> associated with the <see cref="Degree"/>
        /// </summary>
        public int AllModuleCredits => this.AllModules?.Select(x => x?.Credits).Sum() ?? 0;

        /// <summary>
        /// Each module that will count towards the <see cref="Degree"/>
        /// </summary>
        public IList<Module> CountingModules { get; set; }

        /// <summary>
        /// The sum of the Credits for each of the <see cref="CountingModules"/>
        /// </summary>
        public int CurrentCountingModuleCredits => this.CountingModules?.Select(x => x?.Credits).Sum() ?? 0;

        /// <summary>
        /// The <see cref="Module"/> objects that will be used in the 'Quality Assurance' calculation
        /// </summary>
        public IList<Module> QualityAssuranceModules { get; set; }

        /// <summary>
        /// A <see cref="ClassThresholds"/> object representing the Credits threshold for each classification
        /// </summary>
        public ClassThresholds InitialClassThresholds { get; set; }




        /// <summary>
        /// The sum of the Credits value of each <see cref="Module"/> which has been counted towards the <see cref="Degree"/> and is not transferred credit
        /// </summary>
        public int CountingModuleCreditsForClassification
        {
            get
            {
                var sum = this.CountingModules?
                    .Where(x => x.Grade != Grade.Transferred)
                    .Select(x => x?.Credits).Sum();
                return Convert.ToInt32(sum);
            }
        }

        /// <summary>
        /// The 'initial' honours classification based on the <see cref="DegreeWeightedCredits"/> and <see cref="ClassThresholds"/> 
        /// </summary>
        public HonoursClass InitialClass
        {
            get
            {
                if (InitialClassThresholds is not null && DegreeWeightedCredits > 0)
                {
                    if (DegreeWeightedCredits <= InitialClassThresholds.FirstUpperBound) return HonoursClass.First;
                    if (DegreeWeightedCredits <= InitialClassThresholds.UpperSecondUpperBound) return HonoursClass.UpperSecond;
                    if (DegreeWeightedCredits <= InitialClassThresholds.LowerSecondUpperBound) return HonoursClass.LowerSecond;
                    if (DegreeWeightedCredits <= InitialClassThresholds.ThirdUpperBound) return HonoursClass.Third;
                }
                return HonoursClass.Unclassified;
            }
            private set { }
        }

        /// <summary>
        /// The classification indicated by the 'quality assurance' test
        /// </summary>
        public HonoursClass QualityAssuranceClass
        {
            get
            {
                if (QualityAssuranceWeightedCredits > 0)
                {
                    if (QualityAssuranceWeightedCredits <= Constants.QAClassThresholds.FirstUpperBound) return HonoursClass.First;
                    if (QualityAssuranceWeightedCredits <= Constants.QAClassThresholds.UpperSecondUpperBound) return HonoursClass.UpperSecond;
                    if (QualityAssuranceWeightedCredits <= Constants.QAClassThresholds.LowerSecondUpperBound) return HonoursClass.LowerSecond;
                    if (QualityAssuranceWeightedCredits <= Constants.QAClassThresholds.ThirdUpperBound) return HonoursClass.Third;
                }
                return HonoursClass.Unclassified;
            }
            private set { }
        }

        /// <summary>
        /// The final overall honours degree classification
        /// </summary>
        public HonoursClass FinalClass { get; set; }


        /// <summary>
        /// Add a module to the <see cref="Degree"/> and reset the classification
        /// </summary>
        /// <param name="module"></param>
        public void AddModule(Module module)
        {
            AllModules.Add(module);
            ResetClassifications();
        }

        /// <summary>
        /// Remove a module from the <see cref="Degree"/> and reset the classification
        /// </summary>
        /// <param name="module"></param>
        public void RemoveModule(Module module)
        {
            AllModules.Remove(module);
            ResetClassifications();
        }



        /// <summary>
        /// Reset all members set when calculating the degree class
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
        public int DegreeWeightedCredits
        {
            get
            {
                var sum = this.CountingModules?
                    .Where(x => x.Grade != Grade.Transferred)
                    .Select(x => x?.FinalWeightedCredits).Sum();
                return sum ?? 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int QualityAssuranceWeightedCredits
        {
            get
            {
                var sum = this.QualityAssuranceModules?
                    .Where(x => x.Grade != Grade.Transferred)
                    .Select(x => x?.GradeWeightedCredits).Sum();
                return sum ?? 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Module> OrderedLevelThreeModules
        {
            get
            {
                return AllModules
                    .Where(x => x.Level == Level.Three)
                    .OrderBy(x => (int)x.Grade)
                    .OrderByDescending(x => x.Compulsory ? 1 : 0);
            }
        }

        public IEnumerable<Module> OrderedCountedLevelThreeModules
        {
            get
            {
                return CountingModules
                    .Where(x => x.Level == Level.Three)
                    .OrderBy(x => (int)x.Grade);
            }
        }

        public IEnumerable<Module> UncountedModules
        {
            get
            {
                return AllModules
                    .Where(x => !CountingModules.Contains(x))
                    .OrderBy(x => (int)x.Grade)
                    .OrderByDescending(x => x.Compulsory ? 1 : 0);
            }
        }

        public string GetModuleCodeString()
        {
            string[] moduleCodes = AllModules.Select(x => x.ModuleDescriptionCode).ToArray();
            return string.Join("&", moduleCodes);
        }


        /// <summary>
        /// Check a valid credit amount is present
        /// </summary>
        /// <returns></returns>
        public void Validate()
        {
            if (this.AllModuleCredits < Constants.RequiredCreditsAboveLevel1)
                this.CalculationResult.Errors.Add($"Not enough total module credits");

            if (this.AllModules?.Where(x => x?.Level == Level.Three).Select(x => x?.Credits).Sum() < Constants.RequiredCreditsLevel3)
                this.CalculationResult.Errors.Add($"Not enough Level 3 credits");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void Classify(IClassifier classifier)
        {
            ResetClassifications();
            Validate();
            if (!CalculationResult.Errors.Any())
            {
                classifier.Classify(this);
            }
        }


        public bool IsValid 
        {
            get
            {
                return this.IsCalculated && !this.CalculationResult.Errors.Any();
            }       
        }


        /// <summary>
        /// Get all modules which will count for the degree class from the given list
        /// </summary>
        /// <param name="availableModules"></param>
        /// <param name="maxCredits"></param>
        /// <param name="doubleWeight"></param>
        public void GetCountingModules(List<Module> availableModules, int maxCredits, bool doubleWeight)
        {
            foreach (var nextModule in availableModules)
            {
                if (CurrentCountingModuleCredits < maxCredits)
                {
                    var nextModuleInitialCredits = nextModule.Credits;
                    if (nextModule.Credits + CurrentCountingModuleCredits > maxCredits)
                    {
                        nextModule.Code += " (Partial)";
                        nextModule.Credits = (maxCredits - CurrentCountingModuleCredits);
                        AllModules.Add(new Module()
                        {
                            Code = nextModule.Code,
                            Credits = nextModuleInitialCredits - nextModule.Credits,
                            Level = nextModule.Level,
                            Compulsory = nextModule.Compulsory,
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
        public void AddQualityAssuranceModules()
        {
            var qaCredits = 0;
            foreach (var module in this.OrderedCountedLevelThreeModules)
            {
                if (qaCredits < Constants.RequiredQualityAssuranceCredits)
                {
                    if ((module.Credits + qaCredits) > Constants.RequiredQualityAssuranceCredits)
                    {
                        this.QualityAssuranceModules.Add(new Module()
                        {
                            Credits = module.Credits - (Constants.RequiredQualityAssuranceCredits - qaCredits),
                            Level = Level.Three,
                            Grade = module.Grade
                        });
                    }
                    else
                    {
                        this.QualityAssuranceModules.Add(module);
                    }
                    qaCredits += module.Credits;
                }
            }
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
            return null;
        }

    }    
}