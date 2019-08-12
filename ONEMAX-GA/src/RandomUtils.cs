using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntCode64.ONEMAX_Console
{
    /// <summary>
    /// Sole purpose is to provide global Random generator, so all other classes may use the same rndGen
    /// </summary>
    static class RandomUtils
    {
        public static Random RndGen { get; set; } = new Random();
    }
}
