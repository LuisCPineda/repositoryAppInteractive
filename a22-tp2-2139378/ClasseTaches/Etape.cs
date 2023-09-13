using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClasseTaches
{
    public class Etape: IXMLSerializable
    {
        
        public Etape(String descriptionEtape,int nbEtape)
        {
            Description=descriptionEtape;
            Nombre=nbEtape;
        }
        public Etape(XmlElement elementEtape)
        {
            FromXML(elementEtape);
        }
        public bool Termine
        {
            get;
            set;
        }
        public int Nombre
        {
            get;
            set;
        }
        public String Description
        {
            get;
            set;
        }

        public void FromXML(XmlElement elem)
        {
            
            Termine = StringToBoolean(elem.GetAttribute("termine"));
            Nombre = Int32.Parse(elem.GetAttribute("no"));
            Description = elem.InnerText.Trim();
            VerifierTerminationEtape();
        }

        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement elementEtape = doc.CreateElement("etape");
            elementEtape.SetAttribute("termine", BooleanToString(Termine));
            elementEtape.SetAttribute("no", Nombre.ToString());
            elementEtape.InnerText = EffacerTerminer(Description);

            return elementEtape;
        }
        private bool StringToBoolean(String termineString)
        {
            bool result = false;
            if (termineString.Equals("True"))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        private string BooleanToString(bool termine)
        {
            string result = "";
            if (termine)
            {
                result = "True";
            }
            else
            {
                result = "False";
            }
            return result;
        }
        public void VerifierTerminationEtape()
        {
            if (Termine)
            {
                Description += " (Terminée)";
            }
        }

        private String EffacerTerminer(String descriptioAEffacer)
        {
            String [] phraseCoupee = descriptioAEffacer.Split(" ");
            String nouvellePhrase="";
            if (Termine)
            {
                for (int i = 0; i < phraseCoupee.Length - 1; i++)
                {
                    nouvellePhrase += phraseCoupee[i]+" ";
                }
            }
            else
            {
                for (int i = 0; i < phraseCoupee.Length; i++)
                {
                    nouvellePhrase += phraseCoupee[i]+" ";
                }
            }
            
            return nouvellePhrase;
        }
    }
}
