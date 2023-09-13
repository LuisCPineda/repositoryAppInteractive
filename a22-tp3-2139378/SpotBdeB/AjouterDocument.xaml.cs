using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using ViewModel;
using Path = System.IO.Path;

namespace SpotBdeB
{
    /// <summary>
    /// Logique d'interaction pour AjouterDocument.xaml
    /// </summary>
    public partial class AjouterDocument : Window
    {

        private string pathFichierDocuments;
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;

        public static RoutedCommand AnnulerOperationCommand = new RoutedCommand();
        public static RoutedCommand SelectionnerCommand = new RoutedCommand();
        private ViewModelMusique _viewModelMusique;

        public AjouterDocument(ViewModelMusique viewModelMusique)
        {
            _viewModelMusique = viewModelMusique;
            InitializeComponent();
        }
        private void AnnulerOperationCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AnnulerOperationCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
        private void SelectionnerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = InputNom.Text!="" && InputArtiste.Text!="";
        }

        private void SelectionnerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string nomFichier = "";
            string[] pathfichier;

            pathFichierDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         DIR_SEPARATOR + "Fichiers-3GP" + DIR_SEPARATOR + "Musique";

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Fichier";
            dialog.DefaultExt = ".mp3";
            dialog.Filter = "mp3 files (.mp3)|*.mp3";

            bool? resultat = dialog.ShowDialog();

            if (resultat.HasValue && resultat.Value)
            {
                nomFichier = dialog.FileName;
                
            }
            

            pathfichier = nomFichier.Split("\\");
            pathFichierDocuments += "\\"+pathfichier[pathfichier.Length - 1];

            File.Copy(nomFichier, pathFichierDocuments, true);

            Piece nouvellePiece = new Piece(InputArtiste.Text, InputNom.Text, pathfichier[pathfichier.Length-1]);
            _viewModelMusique.AjouterNouvellePiece(nouvellePiece);
            Close();
            _viewModelMusique.SauvegarderDocuPlay();
        }
    }
}
