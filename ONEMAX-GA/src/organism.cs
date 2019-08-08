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

        public Organism()
        {
            
        }

        //Initialises an organism with a random genome -- Should only be used when creating a new population
        public void initRandom(int genomeLength)
        {
            for (int i = 0; i < genomeLength; i++)
            {
                int randnum = RandomUtils.rndGen.Next(2);
                genes.Add(randnum);
            }
        }

        //Calculate fitness of this specific organism (basically counts how many 1's appear, then divides to get
        //a float between 0 and 1

        //Only to be used by Population.UpdateFitness(), when getting fitness of organism, use float fitness { get; set; }
        public void UpdateFitness()
        {
            float fitness = 0;
            for (int i = 0; i < this.genes.Count; i++)
            {
                if (genes[i] == 1) fitness++;
            }
            fitness /= this.genes.Count; //Changes fitness to a float from 0 to 1
            this.fitness = fitness;
        } 

        //Takes two parent organisms and creates a child with genes from both, analogous to 'crossover stage'
        //of meiosis
        //Currently only supports two parents
        //Uses uniform crossover, may experiment with single-point crossover and k-point crossover later
        public Organism crossover(Organism B)
        {
            Organism child = new Organism();

            for (int i = 0; i < genes.Count; i++)
            {
                //50:50 odds of getting a gene from either parent
                if (RandomUtils.rndGen.Next(2) == 1)
                    child.genes.Add(B.genes[i]);
                else
                    child.genes.Add(this.genes[i]); //this = parentA
            }

            return child;
        }

        //Mutate this specific organism
        public void mutate(float mutationRate)
        {
            if (mutationRate == 0) return; //Ensures no division by 0 errors
            
            //Does this process PER GENE, not per organism
            for (int i = 0; i < genes.Count; i++)
            {
                if (RandomUtils.rndGen.Next((int)(1/mutationRate)) == 0) //e.g. let mutRate be 0.01, then we get rndGen.Next(100), and there is a 1% of getting a 0.
                {
                    switch (genes[i])
                    {
                    case 0:
                        genes[i] = 1;
                        break;
                    case 1:
                        genes[i] = 0;
                        break;
                    }
                    //genes[i] = RandomUtils.rndGen.Next(2); //Gets a random int, either 0 or 1, and assigns it to that specific gene
                }
            }
        }

        //Returns this.genes as a string
        override public string ToString()
        {
            string output = "";
            for (int i = 0; i < genes.Count; i++) output += genes[i].ToString();

            return output;
        }
    }
}
