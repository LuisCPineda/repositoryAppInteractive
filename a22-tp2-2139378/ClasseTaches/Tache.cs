using System.Data;
using System.Text;
using System.Xml;

namespace ClasseTaches
{   
    public class Tache : IXMLSerializable
    {
        
        public List<Etape> listeEtapes;
        

        public Tache(string descriptionTache)
        {
            listeEtapes=new List<Etape>();
            Description=descriptionTache;
            DateTime date = DateTime.Now;
            Creation= date.ToString("yyyy/MM/dd");
        }

        public Tache(XmlElement elementEtape)
        {
            FromXML(elementEtape);
        }
        public String Creation
        {
            set;
            get;
        }
        public String Debut
        {
            set;
            get;
        }
        public String Fin
        {
            set;
            get;
        }
        public String Description
        {
            set;
            get;
        }

        public void FromXML(XmlElement elem)
        {
            listeEtapes = new List<Etape>();
            Creation = elem.GetAttribute("creation");
            Debut = elem.GetAttribute("debut");
            Fin = elem.GetAttribute("fin");
            Description = elem["description"].InnerText.Trim();
            XmlNodeList nodeList = elem.GetElementsByTagName("etape");
            listeEtapes = new List<Etape>();
            foreach (XmlNode etape in nodeList)
            {
                XmlElement elemEtape = etape as XmlElement;
                Etape newEtape = new Etape(elemEtape);
                listeEtapes.Add(newEtape);
            }
            
        }
        public XmlElement ToXML(XmlDocument doc)// pour sauvgarder
        {
            XmlElement elemTache = doc.CreateElement("tache");
            elemTache.SetAttribute("creation",Creation);
            elemTache.SetAttribute("debut", Debut);
            elemTache.SetAttribute("fin", Fin);

            XmlElement elemDescrip = doc.CreateElement("description");
            elemDescrip.InnerText = Description;

            XmlElement elemEtape = doc.CreateElement("etapes");
            foreach(Etape etape in listeEtapes)
            {
                elemEtape.AppendChild(etape.ToXML(doc));
            }

            elemTache.AppendChild(elemDescrip);
            elemTache.AppendChild(elemEtape);
            
            return elemTache;
            
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Creation: ");
            builder.AppendLine("Début: ");
            builder.AppendLine("Fin: ");
            builder.AppendLine(Description);
            return builder.ToString();
        }
        public void ChangerNombreItem(int nombreItem)
        {
            foreach(Etape etape in listeEtapes)
            {
                if (etape.Nombre > nombreItem)
                {
                    etape.Nombre -= 1;
                }
            }
        }
    }
}