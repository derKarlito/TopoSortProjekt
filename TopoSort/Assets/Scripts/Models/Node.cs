using System;
namespace TopoSort {
public class Node {
    public string name{get; private set;}
    public Node[] Dependencies {get; private set;}
    public Node(string name,params Node[] dependencies){
        this.name = name;
        this.Dependencies = dependencies;
    }
}
}