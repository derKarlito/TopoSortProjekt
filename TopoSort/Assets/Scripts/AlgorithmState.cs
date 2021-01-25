using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace TopoSort
{
    public class AlgorithmState
    { 

        public Node Current;            // The current Node the algorithm is looking at  
        public List<Node> NewSources;   // holds the nodes that are new sources

        public AlgorithmState() 
        {
            this.NewSources = new List<Node>();
        }
    }
}
