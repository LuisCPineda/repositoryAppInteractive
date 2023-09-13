using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Model
{
    public class Piece : IXMLSerializable
    {
        public Piece(XmlElement elem)
        {
            FromXML(elem);
        }
        public Piece(string nomChanson,string artiste,string nomfichier)
        {
            NomChanson=nomChanson;
            Artiste=artiste;
            NomFichier=nomfichier;
        }
        public string NomChanson
        {
            set;
            get;
        }
        public string Artiste
        {
            set;
            get;
        }
        public int IdChanson
        {
            set;
            get;
        }
        public string NomFichier
        {
            set;
            get;
        }
        public override string ToString()
        {
            return Artiste + "-" + NomChanson;
        }
        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement elementPiece = doc.CreateElement("document");
            elementPiece.SetAttribute("id", IdChanson.ToString());
            elementPiece.SetAttribute("titre", NomChanson);
            elementPiece.SetAttribute("artiste", Artiste);
            elementPiece.SetAttribute("fichier", NomFichier);

            return elementPiece;

        }

        public void FromXML(XmlElement elem)
        {
            IdChanson = Int32.Parse(elem.GetAttribute("id"));
            NomChanson = elem.GetAttribute("titre");
            Artiste = elem.GetAttribute("artiste");
            NomFichier = elem.GetAttribute("fichier");
        }
    }
}
