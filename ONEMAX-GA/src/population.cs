﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONEMAX_GA.src
{
    class Population
    {
        //Population Characteristics
        readonly int popSize;
        readonly float mutationRate;
        readonly int numOfParents;

        //Population Statistics
        float avgFitness = 0;
        float topFitness = 0;
        float fitsum = 0;
        int topOrgID;
        int generationsPassed = 1;
        public bool isFinished = false;

        //A list of organisms, which will serve as the population
        List<Organism> orgs = new List<Organism>();

        public Population(int popSize, int genomeLength, float mutationRate, int numOfParents)
        {
            //Sets local variables to the values of the parameters
            this.popSize = popSize;
            this.mutationRate = mutationRate;
            this.numOfParents = numOfParents;

            for (int i = 0; i < popSize; i++)
            {
                //Creates new organism and adds it to the 'orgs' list
                Organism newOrg = new Organism();
                newOrg.initRandom(genomeLength);
                orgs.Add(newOrg);
            }
        }

        public void calcFitness()
        {
            fitsum = 0;
            topFitness = 0;
            for (int i = 0; i < this.orgs.Count; i++)
            {
                //Makes the organism calculate their fitness, and update its fitness field
                this.orgs[i].UpdateFitness();
                
                fitsum += orgs[i].fitness;
                if (topFitness < orgs[i].fitness)
                {
                    topFitness = orgs[i].fitness;
                    topOrgID = i;
                }
            }
            this.avgFitness = fitsum / this.orgs.Count;

            //Ending program if an organism with fitness 1 occurs
            if (topFitness == 1) isFinished = true;
        }

        //Documentation on implementation of roulette selection                 !!!
        /*
            Each organism gets a selection threshold, which is determined by its fitness. You end up with a number
            line, where org 0's selection threshold might be 0.0678, and org 1's is 0.0926, and so on up until the last 
            organism which has a selection threshold of 1 (or near abouts depending on float rounding).
            Then a random number between 0 and 1 is selected. If it is less than organism x's selection
            threshold, and more than the organism x-1's threshold, then organism x is selected. For example, using the
            above numbers, if rndNum is >= 0 and < 0.0678, then organism 0 is selected. If rndNum is >= 0.0678 and < 0.0926,
            organism 1 is selected.

            It should be noted in this example, organism 0 has a higher fitness than organism 1. The gap between 0 and
            0.0678 is larger than the gap between 0.0678 and 0.0926. Therefore, there is a greater chance of rndNum landing
            in the first gap than the second.

            What you are left with is essentially a roulette, where the odds of an organism being selected is directly
            proportional to its fitness. 

            SEE:
            http://geneticalgorithms.ai-depot.com/Tutorial/Overview.html
            https://en.wikipedia.org/wiki/Fitness_proportionate_selection#Pseudocode
         */

        //Calculates the selection threshold for each organism
        public void calcSelectionThresholds()
        {
            //Setup for roulette selection

            //Setting each organism's selection threshold
            //Processes orgs[0] separately to the rest of the population as orgs[0 - 1] causes an indexing error
            this.orgs[0].selectionThreshold = 0 + this.orgs[0].fitness / fitsum; //In this case '0' acts as the previous organism's selection threshold
            for (int i = 1; i < popSize; i++)
            {
                //Takes the previous organism's selection threshold and adds a given number dependent on its fitness
                this.orgs[i].selectionThreshold = this.orgs[i - 1].selectionThreshold + this.orgs[i].fitness / fitsum;
            }
        }

        public Organism selectOrg()
        {
            while (true) //To make sure if it screws up this time because of RNG, it has another go
            {
                //Makes rndNum a random number between 0 and 1 with 3 decimal places
                float rndNum = RandomUtils.rndGen.Next(1000) / 1000f;

                //Processes orgs[0] seperately to the rest of the population as orgs[0 - 1] causes an indexing error
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
            }
        }

        //Makes a child between two organisms (includes organism functions crossover() and mutate()
        public Organism makeChild(Organism A, Organism B)
        {
            Organism childOrg = A.crossover(B);
            childOrg.mutate(this.mutationRate);
            return childOrg;
        }

        //Creates a new generation and sets 'orgs' list to the new organisms
        public void newGen()
        {
            List<Organism> newGeneration = new List<Organism>();

            for (int j = 0; j < popSize / numOfParents; j++)
            {
                Organism parentA = selectOrg();
                Organism parentB = selectOrg();
                while (parentB == parentA) parentB = selectOrg();

                for (int k = 0; k < numOfParents; k++) //DO TWICE
                {
                    Organism childOrg = parentA.crossover(parentB);
                    childOrg.mutate(mutationRate);
                    newGeneration.Add(childOrg);
                }
            }

            orgs.Clear();
            for (int i = 0; i < popSize; i++) orgs.Add(newGeneration[i]);

            generationsPassed++;
        }

        //Prints the genes of every organism in the population
        public void print(bool printPopSample = false)
        {
            //Prints the first 10 organisms of population if requested
            if (printPopSample)
            {
                Console.WriteLine("GENOME - FITNESS - SELECTION THRESHOLD - ID\n");
                for (int i = 0; i < 10; i++) //10 used to be this.popSize
                {
                    for (int j = 0; j < this.orgs[i].genes.Count; j++)
                    {
                        Console.Write(this.orgs[i].genes[j]);
                    }
                    Console.Write("     " + orgs[i].fitness + "     " + orgs[i].selectionThreshold + "     " + i + "\n");
                }
            }

            //Prints population statistics
            Console.WriteLine("Average Fitness: " + this.avgFitness);
            Console.WriteLine("Top Fitness: " + this.topFitness);
            Console.WriteLine("Best Organism ID: " + this.topOrgID);
            Console.WriteLine("Best Organism Genome: " + this.orgs[topOrgID]);
            Console.WriteLine("Generations Passed: " + this.generationsPassed);
        }
    }
}