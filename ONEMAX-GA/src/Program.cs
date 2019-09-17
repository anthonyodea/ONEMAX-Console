using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace AntCode64.ONEMAX_Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Creating local fields used for population initialisation
            int popSize;
            float mutationRate;
            int genomeLength;
            int numOfParents;
            string popName;

            // Only for TryParse() methods
            int parseSuccessful;
            float parseSuccessful_f;

            // Checking command line argument errors

            if (args.Length != 5)
            {
                Console.WriteLine("Incorrect number of parameters entered. Please enter parameters as: popSize genomeLength mutationRate numOfParents popName");
                return;
            }

            // Checking the command line arguments are of the correct type
            //     If yes, set the appropriate variable to that argument
            //     If no, write an error to console and end the program
            if (Int32.TryParse(args[0], out parseSuccessful))
            {
                popSize = Int32.Parse(args[0]);
            }
            else
            {
                Console.WriteLine("popSize must be an integer\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            if (Int32.TryParse(args[1], out parseSuccessful))
            {
                genomeLength = Int32.Parse(args[1]);
            }
            else
            {
                Console.WriteLine("genomeLength must be an integer\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            if (float.TryParse(args[2], out parseSuccessful_f))
            {
                mutationRate = float.Parse(args[2]);
            }
            else
            {
                Console.WriteLine("mutationRate must be a float\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            if (Int32.TryParse(args[3], out parseSuccessful))
            {
                numOfParents = Int32.Parse(args[3]);
            }
            else
            {
                Console.WriteLine("numOfParents must be an integer\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            // popName is already of string type, and so there is no conversion error to watch out for
            popName = args[4];

            // Checking other argument errors
            if (popSize <= 0)
            {
                Console.WriteLine("popSize must be greater than 0");
                return;
            }
            else if (genomeLength <= 0)
            {
                Console.WriteLine("genomeLength must be greater than 0");
                return;
            }
            else if (mutationRate < 0 || mutationRate > 1)
            {
                Console.WriteLine("mutationRate must be between 0 and 1");
                return;
            }
            else if (numOfParents <= 0)
            {
                Console.WriteLine("numOfParents must be greater than 0");
                return;
            }
            else if (popName.Length <= 0)
            {
                Console.WriteLine("popName must have at least 1 character");
                return;
            }
            else if (popSize % numOfParents != 0)
            {
                Console.WriteLine("popSize must be evenly divisible by numOfParents");
                return;
            }

            // Creating TimerData.csv if it does not already exist
            if (!File.Exists("TimerData.csv"))
            {
                File.AppendAllText("TimerData.csv", "Population Name, Population Size, Mutation Rate, Top Fitness, Average Fitness, Time to Completion, CPU Time, Reason for Termination\n");
            }

            // Starting stopwatch for 1 min cutoff
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Population class constructor uses popsize and genomeLength
            Population pop = new Population(popSize, genomeLength, mutationRate, numOfParents, popName);
            
            // Evolutionary loop            
            while (!pop.isFinished)
            {
                // Calculates the fitness of every organism in the population
                pop.UpdateFitness();

                // Calculates the selection thresholds of every organism in the population
                pop.CalcSelectionThresholds();

                // Prints Poplulation data and if parameter 0 is true, then a sample of the population (i.e. pop.print(true))
                // pop.Print();

                // Prints to the data file specific to this population
                pop.PrintToFile();

                // Creates a new generation, and updates pop.orgs to hold the new organisms
                pop.NewGen();

                if (sw.ElapsedMilliseconds >= 60000)
                {
                    PrintTimerData(pop.getFinalResult(), sw.ElapsedMilliseconds, pop.termReason);
                    return;
                }
            }

            sw.Stop();
            PrintTimerData(pop.getFinalResult(), sw.ElapsedMilliseconds, pop.termReason);
            return;
        }

        public static void PrintTimerData(string finalResult, long millelapsed, string termReason)
        {
            double cpuTime = Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds;

            File.AppendAllText("TimerData.csv", finalResult + ", ");
            File.AppendAllText("TimerData.csv", millelapsed.ToString() + ", ");
            File.AppendAllText("TimerData.csv", cpuTime.ToString() + ", ");
            File.AppendAllText("TimerData.csv", termReason + "\n");
        }
    }
}
