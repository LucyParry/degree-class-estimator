using System;
using System.ComponentModel.DataAnnotations;

namespace HonoursClassEstimator.Model
{
    /// <summary>
    /// Represents a module forming part of a <see cref="Degree"/>  
    /// </summary>
    public class Module
    {
        public Module()
        {
            Compulsary = false;
            DoubleWeight = false;
        }

        [Required]
        [MinLength(1, ErrorMessage = "A description, module code or other identifier is required")]
        public string Description { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "Number of points invalid - Must be between 1 and 120")]
        public int Points { get; set; }

        [Required]
        public Level Level { get; set; }

        [Required]
        public Grade Grade { get; set; }


        /// <summary>
        /// The <see cref="Module"/> may be compulsary for the <see cref="Degree"/> or not. Compulsary modules are counted before non-compulsary ones
        /// </summary>
        public bool Compulsary { get; set; }


        /// <summary>
        /// The module may be given double weighting in the final calculation
        /// </summary>
        public bool DoubleWeight { get; set; }


        /// <summary>
        /// Instantiate and return an new module with the same Description, Level, Points and Grade as this one
        /// </summary>
        /// <returns></returns>
        public Module CopyModule()
        {
            var module = new Module
            {
                Description = this.Description,
                Grade = this.Grade,
                Level = this.Level,
                Points = this.Points,
                Compulsary = this.Compulsary
            };
            return module;
        }


        /// <summary>
        /// Check the <see cref="Module"/> is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid() => (this.Points > 0 && this.Points <= Constants.ModuleMaxPoints);


        /// <summary>
        /// The <see cref="Points"/> multiplied by the <see cref="Grade"/> (so a lower <see cref="Grade"/> 'costs' more)
        /// </summary>
        public int GradeWeightedPoints => IsValid() ? (Points * (int) Grade) : -1;


        /// <summary>
        /// The <see cref="GradeWeightedPoints"/> with 'double weighting' if appropriate (usually done for the first 120 points at Level 3)
        /// </summary>
        public int FinalWeightedPoints
        {
            get
            {
                if (IsValid())
                {
                    return DoubleWeight ? (GradeWeightedPoints * 2) : GradeWeightedPoints;
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
                Description = description[0],
                Points = Convert.ToInt32(description[1].Substring(pIndex + 1, (lIndex - 1) - pIndex)),
                Level = (Level)Convert.ToInt32(description[1].Substring(lIndex + 1, (gIndex - 1) - lIndex)),
                Grade = (Grade)Convert.ToInt32(description[1].Substring(gIndex + 1, (cIndex - 1) - gIndex)),
                Compulsary = Convert.ToInt32(description[1].Substring(cIndex + 1)) == 0 ? false : true
            };
        }


        /// <summary>
        /// Short code which represents the details of the <see cref="Module"/>
        /// </summary>
        public string ModuleDescriptionCode => $"{Description}_P{Points}L{(int)Level}G{(int)Grade}C{(Compulsary ? 1 : 0)}";


        /// <summary>
        /// Short numerical description of the grade weighted points
        /// </summary>
        public string GradeWeightedPointsShortDescription => $"{Points} x {(int)Grade}";


        /// <summary>
        /// Short numerical description of the final weighted points calculation
        /// </summary>
        public string FinalWeightedPointsShortDescription
        {
            get
            {
                string doubleWeight = DoubleWeight ? "2 x " : "";
                string gradeString = this.Grade == Grade.Transferred ? "Transferred" : $"x {(int)Grade}";
                return $"{doubleWeight}({Points} {gradeString})";
            }
        }

        /// <summary>
        /// Longer description of the final weighting calculation
        /// </summary>
        public string FinalWeightedPointsLongDescription
        {
            get
            {
                string doubleWeight = DoubleWeight ? ", double weighted as this is a Level 3 module" : "";
                if (this.Grade == Grade.Transferred)
                {
                    return $"{Points} transferred module points";
                }
                return $"{Points} module points, multiplied by {(int)Grade} for grade {(int)Grade}{doubleWeight}";
            }
        }
    }
}