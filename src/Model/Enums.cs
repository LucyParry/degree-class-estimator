using System.ComponentModel;

namespace HonoursClassEstimator.Model
{
    /// <summary>
    /// The classification of a <see cref="Degree"/> using the standard UK First - Third method - Unclassified is default
    /// for <see cref="Degree"/> objects which haven't yet been calculated
    /// </summary>
    public enum HonoursClass
    {
        [Description(description: "First")]
        First = 1,
        [Description(description: "Upper Second (2:1)")]
        UpperSecond = 2,
        [Description(description: "Lower Second (2:2)")]
        LowerSecond = 3,
        [Description(description: "Third")]
        Third = 4,
        [Description(description: "Unclassified")]
        Unclassified = -1
    }

    /// <summary>
    /// An OU <see cref="Module"/> is awarded one of the grades shown, the numbers determine the weighting given to the module
    /// in the <see cref="Degree"/> classification
    /// </summary>
    public enum Grade
    {
        [Description(description: "Distinction")]
        Distinction = 1,
        [Description(description: "Grade 2")]
        Grade2 = 2,
        [Description(description: "Grade 3")]
        Grade3 = 3,
        [Description(description: "Grade 4")]
        Grade4 = 4,
        [Description(description: "Transferred")]
        Transferred = 5
    }

    /// <summary>
    /// An OU <see cref="Module"/> may be Level 1, 2 or 3 - Only 2 or 3 are considered however for the <see cref="Degree"/> classification
    /// </summary>
    public enum Level
    {
        Two = 2,
        Three = 3
    }
}
