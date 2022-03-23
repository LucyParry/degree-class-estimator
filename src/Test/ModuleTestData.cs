using DegreeClassEstimator.Model;
using System;
using System.Collections.Generic;

namespace DegreeClassEstimator.Tests
{
    
    public static class ModuleTestData
    {
        /// <summary>
        /// Genenerate a list of modules for each available Grade and Level, and the most common Credits values
        /// </summary>
        public static List<Module> GetTestData()
        {
            var modules = new List<Module>(); 

            int gradeCount = Enum.GetNames(typeof(Grade)).Length;
            int levelCount = Enum.GetNames(typeof(Level)).Length;
            int[] Credits = { 30, 60, 120 };

            foreach (int pointValue in Credits)
            {
                for (int i = 2; i < levelCount + 2; i++)
                {
                    for (int j = 0; j < gradeCount; j++)
                    {
                        modules.Add(new Module
                        {
                            Code = $"P{pointValue}L{i}G{j}C0",
                            Credits = pointValue,
                            Level = (Level)i,
                            Grade = (Grade)j,
                            Compulsory = false
                        });
                    }
                }
            }

            modules.Add(new Module
            {
                Code = "InvalidCredits",
                Credits = 99999999,
                Level = Level.Three,
                Grade = Grade.Transferred,
                Compulsory = false
            });

            return modules;
        }

    }
}
