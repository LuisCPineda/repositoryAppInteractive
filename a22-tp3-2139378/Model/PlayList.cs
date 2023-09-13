using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    public class PlayList : IXMLSerializable
    {
        
        public ObservableCollection<int> LesIdDesPlaylist
        {
            get;
            set;
        }
        public List<Piece> PieceDansPlaylist
        {
            get;
            set;
        }
        public PlayList(string nomPlaylist)
        {
            NomPlayList=nomPlaylist;
            LesIdDesPlaylist = new ObservableCollection<int>();
            PieceDansPlaylist = new List<Piece>();
        }
        public PlayList(XmlElement elem)
        {
            LesIdDesPlaylist = new ObservableCollection<int>();
            PieceDansPlaylist = new List<Piece>();
            FromXML(elem);
        }

        public string NomPlayList
        {
            get;
            set;
        }
        public string V { get; }

        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement elementPlaylist = doc.CreateElement("liste");
            elementPlaylist.SetAttribute("nom",NomPlayList);
            foreach(int id in LesIdDesPlaylist)
            {
                XmlElement nouveauId = doc.CreateElement("document");
                nouveauId.SetAttribute("id", id.ToString());
                elementPlaylist.AppendChild(nouveauId);
            }
            return elementPlaylist;
        }

        public void FromXML(XmlElement elem)
        {
            LesIdDesPlaylist = new ObservableCollection<int>();
            NomPlayList = elem.GetAttribute("nom");
            XmlNodeList nodeList = elem.GetElementsByTagName("document");
            foreach(XmlNode node in nodeList)
            {
                XmlElement nodeElement = node as XmlElement;
                LesIdDesPlaylist.Add(Int32.Parse(nodeElement.GetAttribute("id")));
            }
        }
        public override string ToString()
        {
            return NomPlayList;
        }

        internal void AjouterPieceDansPlaylist(Piece unPiece)
        {
            PieceDansPlaylist.Add(unPiece);
        }

        internal void AjouterToutesLesPieces(List<Piece> lesPieces)
        {
            foreach(Piece piece in lesPieces)
            {
                LesIdDesPlaylist.Add(piece.IdChanson);
            }
        }

        public void AjouterPieceDansPlaylistEtId(Piece? unPiece)
        {
            PieceDansPlaylist.Add(unPiece);
            LesIdDesPlaylist.Add(unPiece.IdChanson);
        }

        public void EnleverPieceDansPlaylistEtId(int selectedIndex)
        {
            PieceDansPlaylist.RemoveAt(selectedIndex);
            LesIdDesPlaylist.RemoveAt(selectedIndex);
        }
    }
}
