using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ViewModel;

namespace SpotBdeB
{
    public partial class FenetreModification : Window
    {
        private ViewModelMusique _viewModelMusique;

        public static RoutedCommand PasserPieceCommand = new RoutedCommand();
        public static RoutedCommand EnleverCommand = new RoutedCommand();
        public static RoutedCommand ConfirmerCommand = new RoutedCommand();

        public FenetreModification(ViewModelMusique viewModelMusique)
        {
            _viewModelMusique = viewModelMusique;
            InitializeComponent();
            DataContext = _viewModelMusique;
            _viewModelMusique.MiseAJourList();
        }

        private void ListBoxDocuDispo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PasserPieceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListBoxDocuDispo.SelectedItem != null;
        }

        private void PasserPieceCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModelMusique.AjouterPieceDansPlaylist(ListBoxDocuDispo.SelectedItem);
            _viewModelMusique.SauvegarderDocuPlay();
        }
        private void EnleverCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ListBoxPlaylist!=null && ListBoxPlaylist.SelectedItem != null;
        }

        private void EnleverCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModelMusique.EnleverPieceDansPlaylist(ListBoxPlaylist.SelectedIndex);
            _viewModelMusique.SauvegarderDocuPlay();
        }
        private void ConfirmerCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ConfirmerCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModelMusique.SauvegarderDocuPlay();
            Close();
        }
    }
}
