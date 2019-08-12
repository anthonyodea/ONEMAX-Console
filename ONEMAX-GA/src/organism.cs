using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntCode64.ONEMAX_Console
{
    class Organism
    {
        /// <summary>
        /// Creates a list of genes with getter and setter functions
        /// </summary>
        public List<int> genes { get; set; } = new List<int>();
        public float fitness { get; set; }
        public float selectionThreshold { get; set; }

        public Organism()
        {
            
        }

        /// <summary>
        /// Initialises an organism with a random genome -- Should only be used when creating a new population
        /// </summary>
        public void InitRandom(int genomeLength)
        {
            for (int i = 0; i < genomeLength; i++)
            {
                int randnum = RandomUtils.RndGen.Next(2);
                genes.Add(randnum);
            }
        }

        /// <summary>
        /// Calculate fitness of this specific organism (basically counts how many 1's appear, then divides to get
        /// a float between 0 and 1
        /// Only to be used by Population.UpdateFitness(), when getting fitness of organism, use float fitness { get; set; }
        /// </summary>
        public void UpdateFitness()
        {
            float fitness = 0;
            for (int i = 0; i < this.genes.Count; i++)
            {
                if (genes[i] == 1) fitness++;
            }
            fitness /= this.genes.Count; // Changes fitness to a float from 0 to 1
            this.fitness = fitness;
        }

        /// <summary>
        /// Takes two parent organisms and creates a child with genes from both, analogous to 'crossover stage'
        /// of meiosis
        /// Currently only supports two parents
        /// Uses uniform crossover, may experiment with single-point crossover and k-point crossover later
        /// </summary>
        public Organism Crossover(Organism B)
        {
            Organism child = new Organism();

            for (int i = 0; i < genes.Count; i++)
            {
                // 50:50 odds of getting a gene from either parent
                if (RandomUtils.RndGen.Next(2) == 1)
                {
                    child.genes.Add(B.genes[i]);
                }
                else
                {
                    child.genes.Add(this.genes[i]); // this = parentA
                }
            }

            return child;
        }

        /// <summary>
        /// Mutate this specific organism
        /// </summary>
        public void Mutate(float mutationRate)
        {
            if (mutationRate == 0) return; // Ensures no division by 0 errors
            
            // Does this process PER GENE, not per organism
            for (int i = 0; i < genes.Count; i++)
            {
                if (RandomUtils.RndGen.Next((int)(1/mutationRate)) == 0) // e.g. let mutRate be 0.01, then we get rndGen.Next(100), and there is a 1% of getting a 0.
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
                }
            }
        }

        /// <summary>
        /// Returns this.genes as a string
        /// </summary>
        override public string ToString()
        {
            string output = "";
            for (int i = 0; i < genes.Count; i++)
            {
                output += genes[i].ToString();
            }
                
            return output;
        }
    }
}