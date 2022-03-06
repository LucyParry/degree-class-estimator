using DegreeClassEstimator.Model;
using System;
using System.Collections.Generic;

namespace DegreeClassEstimator.Tests
{
    
    public static class ModuleTestData
    {
        /// <summary>
        /// Genenerate a list of modules for each available Grade and Level, and the most common Points values
        /// </summary>
        public static List<Module> GetTestData()
        {
            var modules = new List<Module>(); 

            int gradeCount = Enum.GetNames(typeof(Grade)).Length;
            int levelCount = Enum.GetNames(typeof(Level)).Length;
            int[] points = { 30, 60, 120 };

            foreach (int pointValue in points)
            {
                for (int i = 2; i < levelCount + 2; i++)
                {
                    for (int j = 0; j < gradeCount; j++)
                    {
                        modules.Add(new Module
                        {
                            Description = $"P{pointValue}L{i}G{j}C0",
                            Points = pointValue,
                            Level = (Level)i,
                            Grade = (Grade)j,
                            Compulsary = false
                        });
                    }
                }
            }

            modules.Add(new Module
            {
                Description = "InvalidPoints",
                Points = 99999999,
                Level = Level.Three,
                Grade = Grade.Transferred,
                Compulsary = false
            });

            return modules;
        }

    }
}
