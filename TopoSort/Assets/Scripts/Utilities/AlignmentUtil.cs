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
        int currentPosY = -4;

        private int lastX;
        int lastY;

        public static bool finished = false;

        public int LastX {
            get{
                return lastX;
            }
            set{
                lastX = value;
                if (currentPosY > -4) currentPosY -= 2;
                return;
            }
        }
        public void SortNotesInArrs(){
            finished = false;
            List<List<Node>> colums = new List<List<Node>>();
            List<Node> tempColumn = new List<Node>();
            List<Node> alreadyPlaced = new List<Node>();
            int j = 0;
            colums.Add(new List<Node>());
            for (int i = 0; i < sorted.Count; i++ ){
                if(i != 0 && colums[j].Count > 3){
                    j++;
                    colums.Add(new List<Node>());
                }
                if (sorted[i].Ancestors.Count == 0){ 
                    colums[j].Add(sorted[i]);
                    alreadyPlaced.Add(sorted[i]);
                    Debug.Log("Added"+sorted[i]+"to list 0");
                }
                else if(i != 0 && !sorted[i].Ancestors.Contains(sorted[i-1]) && DependenciesAlreadyResolved(sorted[i],alreadyPlaced)){
                    colums[j].Add(sorted[i]);
                    alreadyPlaced.Add(sorted[i]);
                    Debug.Log("Added"+sorted[i]+"to list "+j);
                }                
                else if(i != 0 && (sorted[i].Ancestors.Contains(sorted[i-1]) || DependenciesAlreadyResolved(sorted[i],alreadyPlaced))){
                    j++;
                    colums.Add(new List<Node>());
                    colums[j].Add(sorted[i]);
                    alreadyPlaced.Add(sorted[i]);
                    Debug.Log("Added"+sorted[i]+"to list "+j);
                }

            }
            currentPosX = -6;
            currentPosY = -4;
            foreach(List<Node> list in colums){
                Debug.Log("List loop, length : "+list.Count);
                    for(int i = 0; i <= list.Count - 1 ; i++){
                        Debug.Log(list[i].Id);
                        GameObject currentNodeObject = GameObject.Find(list[i].Id.ToString());
                        currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);
                        currentPosY += 2;
                    }
                    currentPosY = -4;
                    currentPosX += 2;
            }
            finished = true;
            VisuellFeedback feedback = new VisuellFeedback();
            feedback.ColourProcess(sorted);
        }
        private bool DependenciesAlreadyResolved(Node node, List<Node> alreadyResolved){
            bool resolved = true;
            foreach(Node item in node.Ancestors){
                if(!alreadyResolved.Contains(item)){
                    resolved = false;
                }
            }
            return resolved;
        }
        public void PositionNodes(){
            for(int i = 0; i < sorted.Count; i++){
                GameObject currentNodeObject = GameObject.Find(sorted[i].Id.ToString());
                if(i != 0 && i < sorted.Count -1 && !sorted[i].Descendants.Contains(sorted[i+1]) && !sorted[i].Descendants.Contains(sorted[i-1])){
                    currentPosY += 2;    
                    currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);
                    lastY = currentPosY;
                }
                if(i != 0 && sorted[i].Ancestors.Contains(sorted[i-1])){
                    currentPosX += 2;
                    currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);
                    lastX = currentPosX;
                }

                else{
                    if(i != 0)
                        currentPosX += 2;
                    currentNodeObject.transform.position = new Vector2(currentPosX,currentPosY);
                    LastX = currentPosX;
                }            
        }
    }      
}
}