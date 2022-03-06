using DegreeClassEstimator.Model;
using System.Collections.Generic;
using System.Linq;

namespace DegreeClassEstimator.Tests
{
    public class DegreeTestData
    {
        public List<Module> TestModules { get; set; }

        public DegreeTestData()
        {
            this.TestModules = ModuleTestData.GetTestData();
        }

        public Degree OUDocWorkedExample1
        {
            get 
            {
                return new Degree()
                {
                    AllModules = new List<Module>()
                    {
                        this.TestModules.FirstOrDefault(x => x.Description == "P60L3G3C0").CopyModule(), // L310
                        this.TestModules.FirstOrDefault(x => x.Description == "P60L3G2C0").CopyModule(), // E300
                        this.TestModules.FirstOrDefault(x => x.Description == "P60L2G3C0").CopyModule(), // L211
                        this.TestModules.FirstOrDefault(x => x.Description == "P60L2G3C0").CopyModule(), // U210
                    }
                };
            }
        }

        public Degree OUDocWorkedExample2
        {
            get
            {
                return new Degree()
                {
                    AllModules = new List<Module>()
                    {
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G3C0").CopyModule(), // M381
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G2C0").CopyModule(), // MT365
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G3C0").CopyModule(), // M346
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G1C0").CopyModule(), // M343

                        this.TestModules.FirstOrDefault(x => x.Description == "P60L2G2C0").CopyModule(), // MST209
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L2G1C0").CopyModule(), // MS221
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L2G2C0").CopyModule(), // M249
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L2G2C0").CopyModule(), // M248
                    }
                };
            }
        }

        public Degree WorkedExample3
        {
            get
            {
                return new Degree()
                {
                    AllModules = new List<Module>()
                    {
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L2G1C0").CopyModule(), // M255
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L2G2C0").CopyModule(), // M263
                        this.TestModules.FirstOrDefault(x => x.Description == "P60L2G2C0").CopyModule(), // MST209

                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G3C0").CopyModule(), // M359
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G3C0").CopyModule(), // M362
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G4C0").CopyModule(), // MS325
                        this.TestModules.FirstOrDefault(x => x.Description == "P30L3G2C0").CopyModule(), // MT365


                    }
                };
            }
        }

    }
}
