using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntCode64.ONEMAX_Console
{
    /// <remarks> Documentation on implementation of Random object
    /// 
    /// The same Random object should be used for every random operation in a given context. The Random() constructor
    /// takes the time(Environment.TickCount) as a seed.Therefore, if two Random objects are created at times very close
    /// to each other (and therefore Environment.TickCount has not yet updated), the will have the same seed and therefore
    /// the same output.This first became an issue when trying to generate random organisms; the same organism was generated
    /// multiple times before Environment.TickCount updated.
    /// 
    /// A Random object's output changes depending on how many times the Next() method has been used. Therefore, using the same
    /// object repeatedly should preserve randomness.
    /// 
    /// A static class RandomUtils was created and holds the static Random object 'rndGen', which may be used by all classes.
    /// A static class is one where an instance of the class is not necessary; its variables and methods may be used nonetheless.
    /// </remarks>

    /// <summary>
    /// Sole purpose is to provide global Random generator, so all other classes may use the same rndGen
    /// </summary>
    static class RandomUtils
    {
        public static Random RndGen { get; set; } = new Random();
    }
}
