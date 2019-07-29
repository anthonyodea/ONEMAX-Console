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

        //Organism constructor - takes a random generator passed from population constructor, and uses it to generate
        //a random digit (0 or 1), then adds it to the 'genes' List. 
        public Organism(Random rndGen, int genomeLength)
        {
            for (int i = 0; i < genomeLength; i++)
            {
                int randnum = rndGen.Next(2);
                genes.Add(randnum);
            }
        }
    }
}
