using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using System.Linq;

namespace TopoSort {
    public class AlignmentUtil{
        public List<Node> input;
        public List<Node> sorted;

        float currentPosX = -3.5f;
        float currentPosY = -4;

        private float lastX;
        float lastY;

        public static bool finished = false;


        public void ImprovedGraphVisualisation(){
            bool newCol = false;
            List<Node> alreadyPlaced = new List<Node>();
            List<Node> lastColum = new List<Node>();
            List<Node> thisColumn = new List<Node>();
            for(int i = 0; i< sorted.Count;i++){
                var currentNode = sorted[i];
                // No Ancestors or all dependencies are resolved and there are nor ancestors of current node in this column -> Stack it
                if ((DependenciesAlreadyResolved(currentNode,alreadyPlaced) && !thisColumn.Any(item => currentNode.Ancestors.Contains(item)))){
                    if(newCol)
                        currentPosY+=2;
                    newCol = false;
                    MoveNode(currentNode);
                    currentPosY += 2;
                    alreadyPlaced.Add(currentNode);
                    thisColumn.Add(currentNode);
                }
                // this column contains ancestor of current Node -> move one column further 
                if(currentNode.Ancestors.Any(item => thisColumn.Contains(item))){
                    lastColum = thisColumn;
                    thisColumn = new List<Node>();
                    currentPosX += 2;
                    currentPosY = -4;
                    MoveNode(currentNode);
                    thisColumn.Add(currentNode);
                    alreadyPlaced.Add(currentNode);
                    newCol = true;
                }
            }
            finished = true;
            VisuellFeedback feedback = new VisuellFeedback();
            feedback.ColourProcess(sorted);
        }
        private void MoveNode(Node node){
            GameObject currentNodeObject = GameObject.Find(node.Id.ToString());
            var nodeControl = currentNodeObject.GetComponent<NodeControl>();
            nodeControl.targetPosition = new Vector2(currentPosX,currentPosY);
            nodeControl.moveNode = true; 
                 
        }
        public void SortNotesInArrs(){
            List<List<Node>> colums = new List<List<Node>>(); // List of Columns (Idx 0 first column idx 1 second etc.)
            List<Node> alreadyPlaced = new List<Node>();       // list of already placed notes
            int j = 0;                                         //list counter
            colums.Add(new List<Node>());                      // add first column
            finished = false;
            for (int i = 0; i < sorted.Count; i++ ){
                if(i != 0 && colums[j].Count > 3){             //if current column has more than 3 items, add new column to keep graph ongoing and not stacking to many nodes
                    j++;
                    colums.Add(new List<Node>());
                }
                if (sorted[i].Ancestors.Count == 0){        // all nodes without ancestors are in first colum
                    colums[j].Add(sorted[i]);
                    alreadyPlaced.Add(sorted[i]);
                    Debug.Log("Added"+sorted[i]+"to list 0");
                }
                else if(i != 0 && !sorted[i].Ancestors.Contains(sorted[i-1]) && DependenciesAlreadyResolved(sorted[i],alreadyPlaced)){  // if last placed node is not ancestor of current node
                    colums[j].Add(sorted[i]);                                                                                           // and all dependencies are resolved, add to same column
                    alreadyPlaced.Add(sorted[i]);
                    Debug.Log("Added"+sorted[i]+"to list "+j);
                }                
                else if(i != 0 && (sorted[i].Ancestors.Contains(sorted[i-1]) || DependenciesAlreadyResolved(sorted[i],alreadyPlaced))){ // add new column if all dependecies of a node are already resolved
                    j++;                                                                                                                // or if last set node is ancestor of current node
                    colums.Add(new List<Node>());
                    colums[j].Add(sorted[i]);
                    alreadyPlaced.Add(sorted[i]);
                    Debug.Log("Added"+sorted[i]+"to list "+j);
                }

            }
            currentPosX = -3.5f;
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
        //Check if all ancestors of a node are already resolved
        private bool DependenciesAlreadyResolved(Node node, List<Node> alreadyResolved){
            bool resolved = true;
            foreach(Node item in node.Ancestors){
                if(!alreadyResolved.Contains(item)){
                    resolved = false;
                }
            }
            return resolved;
        }     
    }
}