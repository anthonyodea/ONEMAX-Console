using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONEMAX_GA.src
{
    class Organism
    {
        //Creates a list of genes with getter and setter functions
        public List<int> genes { get; set; } = new List<int>();
        public float fitness { get; set; }
        public float selectionThreshold { get; set; }

        //Organism constructor - takes a random generator passed from population constructor, and uses it to generate
        //a random digit (0 or 1), then adds it to the 'genes' List. 
        //Method retrieved from https://stackoverflow.com/questions/31717160/set-default-value-for-parameter-of-liststring-type-in-function
        public Organism()
        {

        }

        //Initialises an organism with a random genome -- Should only be used when creating a new population
        public void initRandom(int genomeLength)
        {
            for (int i = 0; i < genomeLength; i++)
            {
                int randnum = Global.rndGen.Next(2);
                genes.Add(randnum);
            }
        }

        //Calculate fitness of this specific organism
        //Only to be used by Population.calcFitness(), when getting fitness of algorithm, use float fitness { get; set; }
        public void calcFitness()
        {
            float fitness = 0;
            for (int i = 0; i < this.genes.Count; i++)
            {
                if (genes[i] == 1) fitness++;
            }
            fitness /= this.genes.Count; //Changes fitness to a float from 0 to 1
            this.fitness = fitness;
        } 

        //Currently only supports two parents
        public Organism crossover(Organism B)
        {
            Organism child = new Organism();
            for (int i = 0; i < genes.Count; i++)
            {
                if (Global.rndGen.Next(2) == 1)
                    child.genes.Add(B.genes[i]);
                else
                    child.genes.Add(this.genes[i]); //this = parentA
            }

            return child;
        }

        //Mutate this specific organism
        public void mutate(float mutationRate)
        {
            if (mutationRate == 0) return;
            for (int i = 0; i < genes.Count; i++)
            {
                if (Global.rndGen.Next((int)(1/mutationRate)) == 0)
                {
                    genes[i] = Global.rndGen.Next(2);
                }
            }
        }

        public string returnGenome()
        {
            string output = "";
            for (int i = 0; i < genes.Count; i++) output += genes[i].ToString();

            return output;
        }
    }
}
