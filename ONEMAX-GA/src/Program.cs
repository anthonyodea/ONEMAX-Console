using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONEMAX_GA.src
{
    class Program
    {
        public static void Main()
        {
            //Setting input parameters for population
            int popSize = 1000;
            int genomeLength = 10;

            //Population class constructor uses popsize and genomeLength
            Population pop = new Population(popSize, genomeLength);
            pop.print();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
