using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System.Linq;

namespace TopoSort {
    public class AlignmentUtil{
        public List<Node> input;
        public List<Node> sorted;

        int currentPosX = -6;
        int currentPosY = 0;

        public void PositionNodes(){
            List<Node> alreadyPositioned = new List<Node>();
            Node graphAncest = new Node("");
            for(int i = 0; i < sorted.Count; i++){
                GameObject currentNodeObject = GameObject.Find(sorted[i].Id.ToString());
                if(i != 0 && i< sorted.Count -1 && sorted[i].Descendants.Contains(sorted[i+1]) && sorted[i-1].Ancestors.Contains(sorted[i])){
                    currentPosY += 2;
                    currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);
                }
                if (i != 0 && i< sorted.Count -1 && sorted[i-1].Descendants.Contains(sorted[i]) && sorted[i+1].Ancestors.Contains(sorted[i-1]) ){
                    currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);                    
                    currentPosY += 2;
                }
                else{
                    currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);
                    currentPosX += 2;
                }

        }
    }      
}
}