using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerConsumer
{
    public class Elephants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Weight { get; set; }
        public string Species { get; set; }

        public Elephants()
        {

        }
        public Elephants(int id, string name, long weight, string species)
        {
            Id = id;
            Name = name;
            Weight = weight;
            Species = species;
        }


    }
}
