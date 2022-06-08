using System;
using System.ComponentModel.DataAnnotations;

namespace DegreeClassEstimator.Model
{
    /// <summary>
    /// Represents an OU module, which forms part of a <see cref="Degree"/>  
    /// </summary>
    public class Module
    {
        /// <summary>
        /// An identifying code for the module
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A description, module code or other identifier is required")]
        public string Code { get; set; }

        /// <summary>
        /// The amount of credits this module contributes towards a full <see cref="Degree"/> 
        /// </summary>
        [Required]
        [Range(1, 120, ErrorMessage = "Credits must be between 1 and 120")]
        public int Credits { get; set; }

        /// <summary>
        /// OU modules are classified with Levels (very roughly corresponding to the year of a standard UK 3-year degree)
        /// Level 2 and 3 modules contributes to the <see cref="Degree"/> classification, and are given different weightings
        /// </summary>
        [Required]
        [Range(typeof(Level), nameof(Level.Two), nameof(Level.Three), ErrorMessage = "Select a level")]
        public Level Level { get; set; }

        /// <summary>
        /// OU Module passing grades are scored as Distinction (Best), Grade 2, Grade 3, and Grade 4 (Worst) 
        /// </summary>
        [Required, EnumDataType(typeof(Grade))]
        public Grade Grade { get; set; }

        /// <summary>
        /// The <see cref="Module"/> may be compulsory for the <see cref="Degree"/> or not. 
        /// Compulsory modules are counted before non-compulsory ones in the <see cref="Degree"/> class calculation
        /// </summary>
        public bool Compulsory { get; set; }

        /// <summary>
        /// The module may be given double weighting in the final calculation
        /// </summary>
        public bool DoubleWeight { get; set; }

        /// <summary>
        /// Instantiate and return an new module with the same Description, Level, Credits and Grade as this one
        /// </summary>
        /// <returns></returns>
        public Module CopyModule()
        {
            var module = new Module
            {
                Code = this.Code,
                Grade = this.Grade,
                Level = this.Level,
                Credits = this.Credits,
                Compulsory = this.Compulsory
            };
            return module;
        }


        /// <summary>
        /// Check the <see cref="Module"/> is valid
        /// </summary>
        /// <returns></returns>
        private bool IsValid() => (this.Credits > 0 && this.Credits <= Constants.ModuleMaxCredits);


        /// <summary>
        /// The <see cref="Credits"/> multiplied by the <see cref="Grade"/> (so a lower <see cref="Grade"/> 'costs' more)
        /// </summary>
        public int GradeWeightedCredits => IsValid() ? (Credits * (int) Grade) : -1;


        /// <summary>
        /// The <see cref="GradeWeightedCredits"/> with 'double weighting' if appropriate (usually done for the first 120 Credits at Level 3)
        /// </summary>
        public int FinalWeightedCredits
        {
            get
            {
                if (IsValid())
                {
                    return DoubleWeight ? (GradeWeightedCredits * 2) : GradeWeightedCredits;
                }
                return -1;
            }
        }


        /// <summary>
        /// Regex pattern for a <see cref="ModuleDescriptionCode"/>
        /// </summary>
        public static string ModuleDescriptionCodePattern => "(([a-zA-Z0-9\\s?]+)_[Pp]([0-9]+)[Ll][23][Gg]-?[0-9][Cc][0-1])";


        /// <summary>
        /// Parse a module description code to produce a full <see cref="Module"/>
        /// </summary>
        public static Module GetModule(string descriptionCode)
        {
            string[] description = descriptionCode.Split('_');
            int pIndex = description[1].IndexOf('P');
            int lIndex = description[1].IndexOf('L');
            int gIndex = description[1].IndexOf('G');
            int cIndex = description[1].IndexOf('C');

            return new Module()
            {
                Code = description[0],
                Credits = Convert.ToInt32(description[1].Substring(pIndex + 1, (lIndex - 1) - pIndex)),
                Level = (Level)Convert.ToInt32(description[1].Substring(lIndex + 1, (gIndex - 1) - lIndex)),
                Grade = (Grade)Convert.ToInt32(description[1].Substring(gIndex + 1, (cIndex - 1) - gIndex)),
                Compulsory = Convert.ToInt32(description[1].Substring(cIndex + 1)) != 0
            };
        }


        /// <summary>
        /// Short code which represents the details of the <see cref="Module"/>
        /// </summary>
        public string ModuleDescriptionCode => $"{Code}_P{Credits}L{(int)Level}G{(int)Grade}C{(Compulsory ? 1 : 0)}";


        /// <summary>
        /// Short numerical description of the grade weighted Credits
        /// </summary>
        public string GradeWeightedCreditsShortDescription => $"{Credits} × {(int)Grade}";


        /// <summary>
        /// Short numerical description of the final weighted Credits calculation
        /// </summary>
        public string FinalWeightedCreditsShortDescription
        {
            get
            {
                string doubleWeight = DoubleWeight ? "2 × " : "";
                string gradeString = this.Grade == Grade.Transferred ? "Transferred" : $"× {(int)Grade}";
                return $"{doubleWeight}({Credits} {gradeString})";
            }
        }

        /// <summary>
        /// Longer description of the final weighting calculation
        /// </summary>
        public string FinalWeightedCreditsLongDescription
        {
            get
            {
                string doubleWeight = DoubleWeight ? ", double weighted for Level 3" : "";
                if (this.Grade == Grade.Transferred)
                {
                    return $"{Credits} transferred module Credits";
                }
                return $"{Credits} module Credits, multiplied by {(int)Grade} (Grade {(int)Grade}){doubleWeight}";
            }
        }
    }
}