using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models;

namespace TopoSort
{
    class AlgorithmState
    {

        /*
         * Source-Queue, Sorted-List und Graph sind static
         */


        public Node Current;            // The current Node the algorithm is looking at  
        public List<Node> NewSources;   // holds the nodes that are new sources

        public AlgorithmState() {
            this.NewSources = new List<Node>();
        }

        /*
         * Schritt vor
         * 
        stepForwards()
            Aktuellen State nehmen
            Algoschritt machen
            neuer state aufn stack pushen
            visuelles updaten
        */

        /*
         * 
         * 
        stepBackwards()
            alter state vom stack poppen
            Änderungen invertiert durchführen
            visuelles updaten
         */
    }
}
