using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Documentation on implementation of Random object                  !!!
/*
    The same Random object should be used for every random operation in a given context. The Random() constructor
    takes the time (Environment.TickCount) as a seed. Therefore, if two Random objects are created at times very close
    to each other (and therefore Environment.TickCount has not yet updated), the will have the same seed and therefore
    the same output. This first became an issue when trying to generate random organisms; the same organism was generated
    multiple times before Environment.TickCount updated.

    A Random object's output changes depending on how many times the Next() method has been used. Therefore, using the same
    object repeatedly should preserve randomness.

    A static class Global was created and holds the static Random object 'rndGen', which may be used by all classes.
    A static class is one where an instance of the class is not necessary; its variables and methods may be used nonetheless.
*/

namespace ONEMAX_GA.src
{
    //Sole purpose is to provide global fields
    static class RandomUtils
    {
        //Global Random so every class may use the same Random object
        public static Random rndGen { get; set; } = new Random();
    }

    class Program
    {
        public static void Main(string[] args)
        {
            bool autoset = false; //For debugging purposes

            int popSize;
            int genomeLength;
            float mutationRate;
            int numOfParents;

            if (autoset)
            {
                popSize = 500;
                genomeLength = 40;
                mutationRate = 0.01f;
                numOfParents = 2;
            }
            else
            {
                //Setting input parameters for population
                Console.Write("Please input the population size (between 100 and 5000): ");
                popSize = Int32.Parse(Console.ReadLine());

                Console.Write("Please input the mutation rate (between 0 and 1): ");
                mutationRate = float.Parse(Console.ReadLine());

                Console.Write("Please input the organism genome length (between 10 and 50): ");
                genomeLength = Int32.Parse(Console.ReadLine());

                Console.Write("Please input the number of parents per organism (must be 2): ");
                numOfParents = Int32.Parse(Console.ReadLine());
            }

            //Population class constructor uses popsize and genomeLength
            Population pop = new Population(popSize, genomeLength, mutationRate, numOfParents);
            
            //Evolutionary loop            
            while (!pop.isFinished)
            {
                System.Threading.Thread.Sleep(100);
                Console.Clear();
                //Calculates the fitness of every organism in the population
                pop.calcFitness();
                //Calculates the selection thresholds of every organism in the population
                pop.calcSelectionThresholds();
                //Prints Poplulation data and if parameter 1 is true, then a sample of the population (i.e. pop.print(true))
                pop.print();
                //Creates a new generation, and updates pop.orgs to hold the new organisms
                pop.newGen();
            }
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
