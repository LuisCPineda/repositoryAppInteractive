using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Model;
using System.IO;
using System.Text;

namespace ViewModel
{
    public class ViewModelMusique : INotifyPropertyChanged
    {

        private string pathFichierDocuments;
        private string pathFichierListes;
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private Lecteur _lecteurMusique;
        private BackgroundWorker _backgroundWorker;   // Un thread pour exécuter la lecture en arrière-plan
        private ModelMusique _modelMusique;
        public bool _play = false;

        private PlayList _PlayListCourante
        {
            get;
            set;
        }
        private Piece _PieceCourante
        {
            set;
            get;
        }
        public string ArtistePieceCourante
        {
            get
            {
                if(_PieceCourante == null)
                {
                    return null;
                }
                return _PieceCourante.Artiste;
            }
        }
        public string NomPieceCourante
        {
            get
            {
                if(_PieceCourante == null)
                {
                    return null;
                }
                return _PieceCourante.NomChanson;
            }
        }
        public string NomPlayListCourante
        {
            get
            {
                if(_PlayListCourante == null)
                {
                    return null;
                }
                return _PlayListCourante.NomPlayList;
            }
        }
        public int IndexPieceCourante
        {
            get;
            set;
        }
        public string IndexPiecePlaylist
        {
            get
            {
                if( _PieceCourante == null)
                {
                    return null;
                }
                return "("+IndexPieceCourante+" de "+_PlayListCourante.PieceDansPlaylist.Count+")";
            }
        }
        public string Volume
        {
            get
            {
                return ""+_lecteurMusique.Volume;
            }
        }

        public ObservableCollection<PlayList>? ListePlaylist
        {
            get
            {
                return new ObservableCollection<PlayList>(_modelMusique.LesPlayList);
            }
        }
        public ObservableCollection<Piece>? PieceDansPlaylist
        {
            get
            {
                if (_PlayListCourante == null)
                {
                    return null;
                }
                return new ObservableCollection<Piece>(_PlayListCourante.PieceDansPlaylist);
            }
        }
        public ObservableCollection<Piece>? ToutesLesPieces
        {
            get
            {
                return new ObservableCollection<Piece>(_modelMusique.LesPieces);
            }
        }
        

        public string TempsRestant => _lecteurMusique.IsPlaying || _lecteurMusique.IsPaused ? _lecteurMusique.GetRemainingTime() : "00:00";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public ViewModelMusique()
        {
            _lecteurMusique = new Lecteur();
            _modelMusique = new ModelMusique();
            _PlayListCourante = null;
            pathFichierDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "Musique\\documents.xml";
            pathFichierListes = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "Musique\\listes_lecture.xml";
            _modelMusique.ChargerFichier(pathFichierDocuments, pathFichierListes);
            _lecteurMusique.Volume += 7;
            OnPropertyChanged("Volume");
        }

        public void SauvegarderDocuPlay()
        {
            pathFichierDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "Musique\\documents.xml";
            pathFichierListes = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "Musique\\listes_lecture.xml";
            _modelMusique.SauvegarderXML(pathFichierDocuments, pathFichierListes);
        }

        public void ChangerPlaylist(object selectedItem)
        {
            _PlayListCourante = selectedItem as PlayList;
            OnPropertyChanged("PieceDansPlaylist");
            OnPropertyChanged("NomPlayListCourante");
        }
        

        #region Code pour faire jouer le fichier audio

        public void Play(Piece? unPiece, int indexPiece)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                          DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "Musique\\"+unPiece.NomFichier;
            _PieceCourante = unPiece;
            IndexPieceCourante = indexPiece+1;
            _play = true;

            _lecteurMusique.Play(path);
            PartirThread();
            OnPropertyChanged();
            OnPropertyChanged("ArtistePieceCourante");
            OnPropertyChanged("NomPieceCourante");
            OnPropertyChanged("NomPlayListCourante");
            OnPropertyChanged("IndexPiecePlaylist");
        }
        public void Pause()
        {
            _play = false;
            _lecteurMusique.Pause();
        }
        public void ResumePlay()
        {
            _play=true;
            _lecteurMusique.UnPause();
        }
        public void Stop()
        {
            _play=false;
            _lecteurMusique.Stop();
        }
        public void AugVolume()
        {
            _lecteurMusique.Volume += 10;
            OnPropertyChanged("Volume");
        }

        public void DimVolume()
        {
            _lecteurMusique.Volume -= 10;
            OnPropertyChanged("Volume");
        }



        private void PartirThread()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.DoWork += Attendre;
            _backgroundWorker.ProgressChanged += PropagerTemps;
            _backgroundWorker.RunWorkerCompleted += ProchaineChanson;
            _backgroundWorker.RunWorkerAsync();
        }

        private void PropagerTemps(object? sender, ProgressChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TempsRestant));
        }


        private void Attendre(object? sender, DoWorkEventArgs e)
        {

            while (_lecteurMusique.IsActive)
            {
                System.Threading.Thread.Sleep(20);
                _backgroundWorker.ReportProgress(0);
            }
        }

        private void ProchaineChanson(object? sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (_play && _PlayListCourante.PieceDansPlaylist.Count != IndexPieceCourante 
                    && _lecteurMusique.GetRemainingTime() == "00:00"
                )
                {
                    Piece unPiece = _PlayListCourante.PieceDansPlaylist[IndexPieceCourante];
                    Play(unPiece, IndexPieceCourante);

                }
            }
            catch
            {
                Stop();
            }
            //Traitement requis quand la chanson est terminée
        }



        #endregion
        public void MiseAJourList()
        {
            OnPropertyChanged();
            OnPropertyChanged("PieceDansPlaylist");
            OnPropertyChanged("NomPlayListCourante");
            OnPropertyChanged("ToutesLesPieces");
        }

        public void AjouterPieceDansPlaylist(object selectedItem)
        {
            Piece unPiece = selectedItem as Piece;
            foreach(PlayList unPlaylist in _modelMusique.LesPlayList)
            {
                if(unPlaylist.NomPlayList == _PlayListCourante.NomPlayList)
                {
                    unPlaylist.AjouterPieceDansPlaylistEtId(unPiece);
                    OnPropertyChanged("PieceDansPlaylist");
                }
            }
            
        }

        public void EnleverPieceDansPlaylist(int selectedIndex)
        {
            
            foreach (PlayList unPlaylist in _modelMusique.LesPlayList)
            {
                if (unPlaylist.NomPlayList == _PlayListCourante.NomPlayList)
                {
                    unPlaylist.EnleverPieceDansPlaylistEtId(selectedIndex);
                    OnPropertyChanged("PieceDansPlaylist");
                }
            }
        }

        public void CreerListe(string nomListe)
        {
            _modelMusique.CreerPlayliste(nomListe);
            OnPropertyChanged("PieceDansPlaylist");
        }

        public void AjouterNouvellePiece(Piece nouvellePiece)
        {
            _modelMusique.AjouterNouvellePiece(nouvellePiece);
            OnPropertyChanged("PieceDansPlaylist");
            OnPropertyChanged("ToutesLesPieces");
        }
    }
}