using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AntCode64.ONEMAX_Console
{
    class Population
    {
        // Population Characteristics
        readonly int popSize;
        readonly float mutationRate;
        readonly int numOfParents;

        // Population Statistics
        float avgFitness = 0;
        float topFitness = 0;
        float fitsum = 0;
        int topOrgID;
        int generationsPassed = 1;
        public bool isFinished = false;

        // A list of organisms, which will serve as the population
        List<Organism> orgs = new List<Organism>();

        // Name of the population, for the purpose of data storage in files (see PrintToFile())
        string popName;

        //Output file for this population
        string outFilePath;

        // For TimerData.csv, which records the end statistics of each population
        public string termReason = "Time Limit Reached"; //Set this to default, changed in UpdateFitness() if other cutoff conditions are met

        /// <summary>
        /// Creates a new population with population characteristics popSize, genomeLength, mutationRate, and numOfParents
        /// Initialises local fields with corresponding parameters
        /// 
        /// Creates a new organism and initialises it with a random genome, then adds it to the 'orgs' List. This is repeated
        /// until orgs reaches the same length as popSize
        /// </summary>
        public Population(int popSize, int genomeLength, float mutationRate, int numOfParents, string popName)
        {
            // Sets local variables to the values of the parameters
            this.popSize = popSize;
            this.mutationRate = mutationRate;
            this.numOfParents = numOfParents;
            this.popName = popName;
            this.outFilePath = @"Data/" + popName + "/" + popName + " (1).csv";
            // If a new file for this new population is not created, it will be appended to the previous population with the same name
            // Therefore a new file must be created
            for (int i = 1; File.Exists(outFilePath); i++)
            {
                outFilePath = @"Data/" + popName + "/" + popName + " (" + i.ToString() + ")" + ".csv";
            }
            

            // Creates the Data directory if it does not already exist
            Directory.CreateDirectory("Data");

            //Creates the popName directory if it does not already exist (grouping populations with the same name together)
            Directory.CreateDirectory("Data/" + popName);

            for (int i = 0; i < popSize; i++)
            {
                // Creates new organism and adds it to the 'orgs' list
                Organism newOrg = new Organism();
                newOrg.InitRandom(genomeLength);
                orgs.Add(newOrg);
            }

            File.AppendAllText(outFilePath, "Population Size: " + popSize + ", Mutation Rate: " + mutationRate + "\n\n");

            File.AppendAllText(outFilePath, "Generation, ");
            File.AppendAllText(outFilePath, "CPU Time Elapsed, ");
            File.AppendAllText(outFilePath, "Average Fitness, ");
            File.AppendAllText(outFilePath, "Top Fitness\n");
        }

        /// <summary>
        /// Updates fitness of every organism in this population by use of Organism.UpdateFitness function
        /// </summary>
        public void UpdateFitness()
        {
            fitsum = 0;
            topFitness = 0;
            for (int i = 0; i < this.orgs.Count; i++)
            {
                // Makes the organism calculate their fitness, and update its fitness field
                this.orgs[i].UpdateFitness();

                fitsum += orgs[i].fitness;
                if (topFitness < orgs[i].fitness)
                {
                    topFitness = orgs[i].fitness;
                    topOrgID = i;
                }
            }
            this.avgFitness = fitsum / this.orgs.Count;

            // Ending program if an organism with fitness 1 occurs
            if (topFitness == 1)
            {
                isFinished = true;
                termReason = "Maximum Fitness";
            }   
            else if (checkForAsymptote())
            {
                isFinished = true;
                termReason = "Asymptotic Fitness";
            }
        }
        public string GetFinalResult()
        {
            return popName + ", " + popSize.ToString() + ", " + mutationRate.ToString() + ", " + topFitness.ToString() + ", " + avgFitness.ToString() + generationsPassed.ToString();
        }

        // CONCERN WITH DATA COLLECTION ****************************
        // The cutoff method assumes all populations with the same parameters have the same termination reason
        // What if two runs of the same population end for different reasons? How do you compare them? Or average them?

        private bool checkForAsymptote()
        {
            return false;
        }

        /// <remarks> Documentation on implementation of roulette selection
        /// 
        /// Each organism gets a selection threshold, which is determined by its fitness. You end up with a number
        /// line, where org 0's selection threshold might be 0.0678, and org 1's is 0.0926, and so on up until the last
        /// organism which has a selection threshold of 1 (or near abouts depending on float rounding).
        /// Then a random number between 0 and 1 is selected.If it is less than organism x's selection
        /// threshold, and more than the organism x-1's threshold, then organism x is selected. For example, using the
        /// above numbers, if rndNum is >= 0 and< 0.0678, then organism 0 is selected.If rndNum is >= 0.0678 and< 0.0926,
        /// organism 1 is selected.
        /// 
        /// It should be noted in this example, organism 0 has a higher fitness than organism 1. The gap between 0 and
        /// 0.0678 is larger than the gap between 0.0678 and 0.0926. Therefore, there is a greater chance of rndNum landing
        /// in the first gap than the second.
        /// 
        /// What you are left with is essentially a roulette, where the odds of an organism being selected is directly
        /// proportional to its fitness.
        /// 
        /// SEE:
        /// http://geneticalgorithms.ai-depot.com/Tutorial/Overview.html
        /// https://en.wikipedia.org/wiki/Fitness_proportionate_selection#Pseudocode
        /// </remarks>

        /// <summary>
        /// Calculates the selection threshold for each organism
        /// </summary>
        public void CalcSelectionThresholds()
        {
            // Setup for roulette selection

            // Setting each organism's selection threshold
            // Processes orgs[0] separately to the rest of the population as orgs[0 - 1] causes an indexing error
            this.orgs[0].selectionThreshold = 0 + this.orgs[0].fitness / fitsum; // In this case '0' acts as the previous organism's selection threshold
            for (int i = 1; i < popSize; i++)
            {
                // Takes the previous organism's selection threshold and adds a given number dependent on its fitness
                this.orgs[i].selectionThreshold = this.orgs[i - 1].selectionThreshold + this.orgs[i].fitness / fitsum;
            }
        }

        /// <summary>
        /// Selects an organism for mating based on selection thresholds
        /// </summary>
        public Organism SelectOrg()
        {
            // Makes rndNum a random number between 0 and 1 with 3 decimal places
            float rndNum = RandomUtils.RndGen.Next(1000) / 1000f;

            // Processes orgs[0] seperately to the rest of the population as orgs[0 - 1] causes an indexing error
            if (rndNum >= 0 && rndNum < orgs[0].selectionThreshold)
            {
                return orgs[0];
            }

            for (int i = 1; i < popSize; i++)
            {
                if (rndNum >= orgs[i - 1].selectionThreshold && rndNum < orgs[i].selectionThreshold)
                {
                    return orgs[i];
                }
            }

            /// <remarks>
            /// If for some reason an organism is not selected above, will return the last organism in the list
            /// Usually occurs where last organism's selection threshold is ~0.99998, not 1, and the rndGen outputs above this threshold
            /// Occurs extremely rarely
            /// </remarks>
            return orgs[popSize - 1];
        }

        /// <summary>
        /// Makes a child between two organisms (includes organism functions crossover() and mutate()
        /// </summary>
        public Organism MakeChild(Organism A, Organism B)
        {
            Organism childOrg = A.Crossover(B);
            childOrg.Mutate(this.mutationRate);
            return childOrg;
        }

        /// <summary>
        /// Creates a new generation and sets 'orgs' list to the new organisms
        /// </summary>
        public void NewGen()
        {
            List<Organism> newGeneration = new List<Organism>();

            for (int j = 0; j < popSize / numOfParents; j++)
            {
                Organism parentA = SelectOrg();
                Organism parentB = SelectOrg();
                while (parentB == parentA)
                {
                    parentB = SelectOrg();
                }
                
                for (int k = 0; k < numOfParents; k++) // DO TWICE
                {
                    Organism childOrg = parentA.Crossover(parentB);
                    childOrg.Mutate(mutationRate);
                    newGeneration.Add(childOrg);
                }
            }

            orgs.Clear();
            for (int i = 0; i < popSize; i++)
            {
                orgs.Add(newGeneration[i]);
            }
            generationsPassed++;
        }

        /// <summary>
        /// Prints the genes of every organism in the population
        /// </summary>
        public void Print(bool printPopSample = false)
        {
            // Slows the program down so the data may be readable on screen. This is unnecessary if the 
            // Print() method is not called, therefore the sleep command is given here
            System.Threading.Thread.Sleep(100);

            // Clears previous data shown on screen
            Console.Clear();

            // Prints the first 10 organisms of population if requested
            if (printPopSample)
            {
                Console.WriteLine("GENOME - FITNESS - SELECTION THRESHOLD - ID\n");
                for (int i = 0; i < 10; i++) // 10 used to be this.popSize
                {
                    for (int j = 0; j < this.orgs[i].genes.Count; j++)
                    {
                        Console.Write(this.orgs[i].genes[j]);
                    }
                    Console.Write("     " + orgs[i].fitness + "     " + orgs[i].selectionThreshold + "     " + i + "\n");
                }
            }

            // Prints population statistics
            Console.WriteLine("Average Fitness: " + this.avgFitness);
            Console.WriteLine("Top Fitness: " + this.topFitness);
            Console.WriteLine("Best Organism ID: " + this.topOrgID);
            Console.WriteLine("Best Organism Genome: " + this.orgs[topOrgID]);
            Console.WriteLine("Generations Passed: " + this.generationsPassed);
        }

        /// <summary>
        /// Prints population characteristics to a separate file given by string outFilePath
        /// </summary>
        public void PrintToFile()
        {
            double cpuTime = Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds;

            File.AppendAllText(outFilePath, this.generationsPassed.ToString() + ", ");
            File.AppendAllText(outFilePath, cpuTime.ToString() + ", ");
            File.AppendAllText(outFilePath, this.avgFitness.ToString() + ", ");
            File.AppendAllText(outFilePath, this.topFitness.ToString() + "\n");
        }
    }
}