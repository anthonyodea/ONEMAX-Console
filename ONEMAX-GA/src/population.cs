using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONEMAX_GA.src
{
    class Population
    {
        //Population Characteristics -- Should be kept static (perhaps use Console.Readkey(_popSize); static popSize = _popSize; _popSize.kill
        int popSize;
        float mutationRate;
        int numOfParents;

        //Population Statistics
        float avgFitness = 0;
        float topFitness = 0;
        float fitsum = 0;
        int bestOrgID;
        int generationsPassed = 1;

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
                this.orgs[i].calcFitness();
                
                fitsum += orgs[i].fitness;
                if (topFitness < orgs[i].fitness)
                {
                    topFitness = orgs[i].fitness;
                    bestOrgID = i;
                }
            }
            this.avgFitness = fitsum / this.orgs.Count;

            if (topFitness == 1) Global.isFinished = true;
        }

        public void calcSelectionThresholds()
        {
            //Setup for roulette selection

            //Setting each organism's selection threshold
            this.orgs[0].selectionThreshold = 0 + this.orgs[0].fitness / fitsum;
            for (int i = 1; i < popSize; i++)
            {
                this.orgs[i].selectionThreshold = this.orgs[i - 1].selectionThreshold + this.orgs[i].fitness / fitsum;
            }
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

        //Makes a child between two organisms (includes organism functions crossover() and mutate()
        public Organism makeChild(Organism A, Organism B)
        {
            Organism childOrg = A.crossover(B);
            childOrg.mutate(this.mutationRate);
            return childOrg;
        }

        public Organism selectOrg()
        {
            while (true) //To make sure if it screws up this time because of RNG, it has another go
            {
                float rndNum = Global.rndGen.Next(1000) / 1000f;

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

        //Prints the genes of every organism in the population
        public void print(bool checkForDuplicates = false)
        {
            /*
            Console.WriteLine("GENOME - FITNESS - SELECTION THRESHOLD - ID\n");
            for (int i = 0; i < 10; i++) //10 used to be this.popSize
            {
                for (int j = 0; j < this.orgs[i].genes.Count; j++)
                {
                    Console.Write(this.orgs[i].genes[j]);
                }
                Console.Write("     " + orgs[i].fitness + "     " + orgs[i].selectionThreshold + "     " + i + "\n");

                //Prints a statement if one organism has the exact same genome as another organism
                if (checkForDuplicates)
                {
                    for (int k = 0; k < i; k++)
                    {
                        //Method retrieved from https://stackoverflow.com/questions/22173762/check-if-two-lists-are-equal
                        if (this.orgs[i].genes.SequenceEqual(this.orgs[k].genes) && i != k)
                        {
                            Console.WriteLine("Organism " + i + " == Organism " + k);
                        }
                    }
                }
            }
            */

            Console.WriteLine("Average Fitness: " + this.avgFitness);
            Console.WriteLine("Top Fitness: " + this.topFitness);
            Console.WriteLine("Best Organism ID: " + this.bestOrgID);
            Console.WriteLine("Best Organism Genome: " + this.orgs[bestOrgID].returnGenome());
            Console.WriteLine("Generations Passed: " + this.generationsPassed);
        }
    }
}