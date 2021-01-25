using System.Collections.Generic;

namespace TopoSort
{
    public class GlossarEntry
    {

        private string Term;
        private List<string> Synonyms;
        private string Explanation;


        public GlossarEntry(string term, List<string> synonyms, string explanation)
        {
            this.Term = term;
            this.Synonyms = synonyms;
            this.Explanation = explanation;
        }
        
        public GlossarEntry() : this("", new List<string>(),"")
        {
            
        }


        public string GetTitle()
        {
            return this.Term;
        }
        
        public void SetTitle(string newTitle)
        {
            this.Term = newTitle;
        }
        

        public string GetExplanation()
        {
            return this.Explanation;
        }
        
        public void SetExplanation(string newExplanation)
        {
            this.Explanation = newExplanation;
        }
        
        public List<string> GetSynonyms()
        {
            return this.Synonyms;
        }
        
        
    }
}