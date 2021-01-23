namespace TopoSort
{
    public class GlossarEntry
    {

        private string Term;
        private string[] Synonyms;
        private string Explanation;


        public GlossarEntry(string term, string[] synonyms, string explanation)
        {
            this.Term = term;
            this.Synonyms = synonyms;
            this.Explanation = explanation;
        }
        
        
    }
}