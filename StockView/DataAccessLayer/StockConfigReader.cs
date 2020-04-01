using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StockView.DataAccessLayer
{
    public class StockConfigReader : IConfigReader
    {
        public List<string> ReadFile(string filepath)
        {
            List<string> symbolList = new List<string>();

            {
                foreach (XElement level1Element in XElement.Load(filepath).Elements("Company"))
                {
                    symbolList.Add(level1Element.Attribute("Symbol").Value);
                }
            }
            return symbolList;
        }

        public void AddtoFile(string filepath, string sym)
        {
            XElement newelement = new XElement("Company", new XAttribute("Symbol", sym));
            var root = XElement.Load(filepath);
            root.Add(newelement);
            root.Save(filepath);
        }
        public void RemoveFromFile(string filepath, string sym)
        {
            //XmlDocument xmlDocument = new XmlDocument();
            //var serializer = new XmlSerializer(typeof(List<CompanyConfig>),
            //                       new XmlRootAttribute("Companies"));
            //using (var stream = new FileStream(filepath,FileMode.OpenOrCreate))
            //{
            //    serializer.Serialize(stream, syms);
            //    xmlDocument.Load(stream);
            //    xmlDocument.Save(filepath);
            //}



            //XElement xmlElements = new XElement("Companies", syms.Select(i => new XElement("Company", new XAttribute("Symbol", i))));
            //var root = XElement.Load(filepath);
            //root.RemoveNodes();
            
            //root.Add(xmlElements);
            //root.Save(filepath);



            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            XmlNodeList nodes = doc.GetElementsByTagName("Company");
            bool shouldremove = false;
            List<XmlNode> ndlst = new List<XmlNode>();
            foreach (XmlNode node in nodes)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    if ((attribute.Name == "Symbol") && (attribute.Value == sym))
                    {
                        ndlst.Add(node);                        
                        break;
                    }
                }                
            }
            foreach(var item in ndlst)
            {
                item.ParentNode.RemoveChild(item);
            }
            doc.Save(filepath);

        }
    }
    
    public class CompanyConfig
    {        
        public string Symbol { get; set; }
    }
}
