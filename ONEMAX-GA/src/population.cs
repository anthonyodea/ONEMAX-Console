using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONEMAX_GA.src
{
    class Population
    {
        int popSize;
        List<Organism> orgs = new List<Organism>();

        public Population(int popSize, int genomeLength)
        {
            //Sets local variable 'this.popSize' to the value of parameter 'popSize' 
            this.popSize = popSize;

            //Creates a random generator 'rndGen'
            Random rndGen = new Random();

            for (int i = 0; i < popSize; i++)
            {
                //Creates new organism and adds it to the 'orgs' list
                Organism newOrg = new Organism(rndGen, genomeLength);
                orgs.Add(newOrg);
            }
        }

        //Prints the genes of every organism in the population
        public void print(bool checkForDuplicates = false)
        {
            for (int i = 0; i < this.popSize; i++)
            {
                for (int j = 0; j < this.orgs[i].genes.Count; j++)
                {
                    Console.Write(this.orgs[i].genes[j]);
                }
                Console.Write("     " + i + "\n");

                //Prints a statement if one organism has the exact same genome as another organism
                if (checkForDuplicates)
                {
                    for (int k = 0; k < i; k++)
                    {
                        //Method retrieved from https://stackoverflow.com/questions/22173762/check-if-two-lists-are-equal
                        if (this.orgs[i].genes.SequenceEqual(this.orgs[k].genes) && i != k)
                        {
                            Console.WriteLine("Organism " + i + " == Organism " + k);
                            //Optional program pause
                            //Console.ReadKey();
                        }
                    }
                }
            }
        }
    }
}