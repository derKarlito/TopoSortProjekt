using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

using TopoSort;

namespace TopoSort
{
    public class TextLoader
    {
        private string Path;  
        public TextLoader(string path) 
        {
            this.Path = path;
        }

        
        public string LoadTutorialPage(int page) 
        {
            String text = "";   //will contain the page number, headline and text
            int count = 0;      //if a node attribute match page, it will increased by 3 
            XmlTextReader xtr = new XmlTextReader(this.Path);

            while (xtr.Read())
            {
                String attr = xtr.GetAttribute("number");
                if (attr != null)
                {
                    if (attr.Equals(page.ToString()))
                    {
                        count += 3;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (count != 0 && count > 0) 
                {
                    if (xtr.NodeType == XmlNodeType.Element && xtr.Name == "headline")
                    {
                        text = text + xtr.ReadElementString();
                    }

                    if (xtr.NodeType == XmlNodeType.Element && xtr.Name == "body")
                    {
                        text = text + xtr.ReadElementString();
                    }
                    count--;
                }
            }
            return text;
        }

        public GlossarEntry LoadGlossarEntry(int id) 
        {
            GlossarEntry entry = new GlossarEntry();
            XmlTextReader xtr = new XmlTextReader(this.Path);
            int count = 0;
            while (xtr.Read()) 
            {

                String attr = xtr.GetAttribute("id");
                if (attr != null)
                {
                    if (attr.Equals(id.ToString()))
                    {
                        count += 4;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }

                if(count != 0)
                {
                    //loading terms
                    if (xtr.NodeType == XmlNodeType.Element && xtr.Name == "term")
                    {
                        entry.SetTitle(xtr.ReadElementString());
                    }

                    // loading synonyms
                    if (xtr.NodeType == XmlNodeType.Element && xtr.Name == "synonyms")
                    {
                        List<string> list = new List<string>();
                        String syn = xtr.ReadElementString();
                        if (!syn.Equals(String.Empty))
                        {
                            if (syn.IndexOf(',') == 0)
                            {
                                list.Add(syn);
                                break;
                            }

                            while (syn.IndexOf(',') > 0)
                            {
                                int index = syn.IndexOf(',');
                                String sub = syn.Substring(0, index);
                                list.Add(sub);
                                syn = syn.Remove(0, index + 1);
                            }

                            list.Add(syn);
                        }

                        entry.SetSynonyms(list);
                    }

                    //loading defintion
                    if (xtr.NodeType == XmlNodeType.Element && xtr.Name == "definition")
                    {
                       entry.SetExplanation(xtr.ReadElementString());
                    }
                    count--;
                }
            }

            return entry;
        }

        public void ChangePath(string path)
        {
            this.Path = path;
        }
         
    }
}
