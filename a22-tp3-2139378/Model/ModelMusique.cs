using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Model
{
    public class ModelMusique : IXMLSerializable
    {
        
        public List<PlayList> LesPlayList
        {
            get;
            private set;
        }
        public List<Piece> LesPieces
        {
            get;
            private set;
        }
        public ModelMusique()
        {
            LesPlayList = new List<PlayList>();
            LesPieces = new List<Piece>();
        }

        public void ChargerFichier(string pathFichierDocuments, string pathFichierListes)
        {
            LesPlayList = new List<PlayList>();
            LesPieces = new List<Piece>();
            XmlDocument document = new XmlDocument();
            document.Load(pathFichierDocuments);
            XmlElement racine = document.DocumentElement;
            FromXML(racine);
            
            XmlDocument documentPlay = new XmlDocument();
            PlayList playlistDocu = new PlayList("Tous les documents");
            playlistDocu.AjouterToutesLesPieces(LesPieces);
            LesPlayList.Add(playlistDocu);

            documentPlay.Load(pathFichierListes);
            XmlElement racinePlayList = documentPlay.DocumentElement;
            XmlNodeList playlist = racinePlayList.GetElementsByTagName("liste");
            foreach (XmlNode playlistElement in playlist)
            {
                XmlElement elem = playlistElement as XmlElement;
                PlayList newPlaylist = new PlayList(elem);
                LesPlayList.Add(newPlaylist);
            }
            InsertPieceIntoPlaylist();
        }

        public void SauvegarderXML(string pathFichierDocuments, string pathFichierListes)
        {
            if(pathFichierDocuments != "")
            {
                XmlDocument document = new XmlDocument();
                XmlElement racine = ToXML(document);
                document.AppendChild(racine);
                document.Save(pathFichierDocuments);
            }
            if(pathFichierListes != "")
            {
                XmlDocument document = new XmlDocument();
                XmlElement racine = document.CreateElement("listes");
                document.AppendChild(racine);

                for(int i = 1; i < LesPlayList.Count; i++)
                {
                    XmlElement elementPlay = LesPlayList[i].ToXML(document);
                    racine.AppendChild(elementPlay);
                }

                document.Save(pathFichierListes);
            }
        }
        

        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement elementPiece = doc.CreateElement("documents");
            foreach (var piece in LesPieces)
            {
                elementPiece.AppendChild(piece.ToXML(doc));
            }
            return elementPiece;
        }

        public void FromXML(XmlElement elem)
        {
            LesPieces = new List<Piece>();
            XmlNodeList lesPieces = elem.GetElementsByTagName("document");
            foreach (XmlNode unPiece in lesPieces)
            {
                XmlElement elemEtape = unPiece as XmlElement;
                Piece nouveauPiece = new Piece(elemEtape);
                LesPieces.Add(nouveauPiece);
            }
        }

        private void InsertPieceIntoPlaylist()
        {
            foreach(PlayList unPlaylist in LesPlayList)
            {
                foreach(int id in unPlaylist.LesIdDesPlaylist)
                {
                    foreach(Piece unPiece in LesPieces)
                    {
                        if (id == unPiece.IdChanson)
                        {
                            unPlaylist.AjouterPieceDansPlaylist(unPiece);
                        }
                    }
                }
            }
        }

        public void CreerPlayliste(string nomListe)
        {
            PlayList nouvellePlaylist = new PlayList(nomListe);
            LesPlayList.Add(nouvellePlaylist);
        }

        public void AjouterNouvellePiece(Piece nouvellePiece)
        {
            nouvellePiece.IdChanson = LesPieces.Count + 1;
            LesPlayList[0].AjouterPieceDansPlaylistEtId(nouvellePiece);
            LesPieces.Add(nouvellePiece);
        }
    }
}
