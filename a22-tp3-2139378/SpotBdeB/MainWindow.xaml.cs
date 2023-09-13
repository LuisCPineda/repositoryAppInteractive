using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModel;
using Microsoft.Win32;
using System.Diagnostics;
using Model;

namespace SpotBdeB
{
    public partial class MainWindow : Window
    {
        public static RoutedCommand PlayCommand = new RoutedCommand();
        public static RoutedCommand PauseCommand = new RoutedCommand();
        public static RoutedCommand StopCommand = new RoutedCommand();
        public static RoutedCommand AjouterDocuCmd = new RoutedCommand();
        public static RoutedCommand CreerListeCmd = new RoutedCommand();
        public static RoutedCommand ModifierListeCmd = new RoutedCommand();
        public static RoutedCommand AugVolumeCmd = new RoutedCommand();
        public static RoutedCommand DimVolumeCmd = new RoutedCommand();
        public static RoutedCommand AProposCmd = new RoutedCommand();


        public Piece _pieceCourantPlay;
        public bool _play = false;

        private ViewModelMusique _viewModelMusique;

        public MainWindow()
        {
            _viewModelMusique = new ViewModelMusique();
            InitializeComponent();
            DataContext = _viewModelMusique;
            
        }
        private void APropos_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("SpotBdeB\nVersion 0.1 \n \nAuteur: Luis Carlos Pineda Rodriguez");
        }

        private void APropos_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PlayCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListBoxPieces.SelectedItem != null && ComboBoxListeLecture.SelectedItem != null;
        }

        private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Piece unPiece = ListBoxPieces.SelectedItem as Piece;
            int indexPiece = ListBoxPieces.SelectedIndex;
            
            if ((_pieceCourantPlay==null || _pieceCourantPlay!=ListBoxPieces.SelectedItem) && !_play)
            {
                _pieceCourantPlay = ListBoxPieces.SelectedItem as Piece;
                _viewModelMusique.Play(unPiece, indexPiece);
            }
            else
            {
                _viewModelMusique.ResumePlay();
            }
            
        }
        private void PauseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListBoxPieces.SelectedItem != null && ComboBoxListeLecture.SelectedItem != null;
        }

        private void PauseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModelMusique.Pause();
            _play=false;
        }
        private void AugVolumeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AugVolumeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModelMusique.AugVolume();
        }
        private void DimVolumeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DimVolumeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModelMusique.DimVolume();
        }
        private void StopCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListBoxPieces.SelectedItem != null && ComboBoxListeLecture.SelectedItem != null;
        }

        private void StopCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            _viewModelMusique.Stop();
            _pieceCourantPlay = null;
            _play = false;
        }

        private void AjouterDocu_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AjouterDocu_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AjouterDocument windowAjouterDocument = new AjouterDocument(_viewModelMusique);
            windowAjouterDocument.Show();
        }
        private void CreerListeCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CreerListeCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WindowCreerListe windowAjouterListe = new WindowCreerListe(_viewModelMusique);
            windowAjouterListe.ShowDialog();
        }
        private void ModifierListeCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ComboBoxListeLecture.SelectedItem != null &&
                ComboBoxListeLecture.SelectedItem.ToString() != "Tous les documents"; ;
        }

        private void ModifierListeCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FenetreModification windowFenetreModification = new FenetreModification(_viewModelMusique);
            windowFenetreModification.Show();
        }

        private void ComboBoxListeLecture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModelMusique.ChangerPlaylist(ComboBoxListeLecture.SelectedItem);
        }
    }
}
