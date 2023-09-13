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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.Win32;
using ClasseTaches;
using System.Diagnostics;
using System.Reflection;

namespace Kanban_BdeB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Tache> listeTaches;//à changer dans le collections
        private string NomFichier;
        //Commande poir le menu ficher
        public static RoutedCommand OuvrirCmd = new RoutedCommand();
        public static RoutedCommand EnregistrerCmd = new RoutedCommand();
        public static RoutedCommand EnregistrerSousCmd = new RoutedCommand();
        //Commandes pour la gestion des taches
        public static RoutedCommand TerminerEtapeCmd = new RoutedCommand();
        public static RoutedCommand SupprimerEtapeCmd = new RoutedCommand();
        public static RoutedCommand AjouterTacheCmd = new RoutedCommand();
        //Commandes pour la gestion des etapes
        public static RoutedCommand AjouterEtapeCmd = new RoutedCommand();
        //Commandes pour la gestion d'edition
        public static RoutedCommand SupprimerTacheCmd = new RoutedCommand();
        //Commandes pour la gestion d'à propos
        public static RoutedCommand AProposCmd = new RoutedCommand();

        public MainWindow()
        {
            listeTaches = new List<Tache>();
            InitializeComponent();
        }

        //Fichier
        private void Ouvrir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OuvrirFichier();
        }
        private void Ouvrir_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void Enregistrer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SauvegarderTaches(NomFichier);
        }

        private void Enregistrer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listeTaches.Count>0;
        }
        //Supprimer Tâche
        private void SupprimerTache_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int indexTache = 0;
            if (ListBoxTachePlanifiees.SelectedItem != null)
            {
                int indexListBox=ListBoxTachePlanifiees.SelectedIndex;
                for(int i = 0; i < listeTaches.Count; i++)
                {
                    if (listeTaches[i].Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                    {
                        indexTache = i;
                    }
                }
                listeTaches.RemoveAt(indexTache);
                ListBoxTachePlanifiees.Items.RemoveAt(indexListBox);
                ListBoxInfoTacheActive.Items.Clear();
            }
            if(ListBoxTacheEnCours.SelectedItem != null)
            {
                int indexListBox = ListBoxTacheEnCours.SelectedIndex;
                for (int i = 0; i < listeTaches.Count; i++)
                {
                    if (listeTaches[i].Description.Equals(ListBoxTacheEnCours.SelectedItem))
                    {
                        indexTache = i;
                    }
                }
                listeTaches.RemoveAt(indexTache);
                ListBoxTacheEnCours.Items.RemoveAt(indexListBox);
                ListBoxInfoTacheActive.Items.Clear();
            }
        }

        private void SupprimerTache_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listeTaches.Count > 0;
        }
        //À propos
        private void APropos_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Kanban BdeB\nVersion 0.1 \n \nAuteur: Luis Carlos Pineda Rodriguez");
        }

        private void APropos_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EnregistrerSous_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            bool? resultat = saveFileDialog.ShowDialog();
            if (resultat.HasValue && resultat.Value)
            {
                NomFichier = saveFileDialog.FileName;
                SauvegarderTaches(NomFichier);
            }
            SauvegarderTaches(NomFichier);
        }

        private void EnregistrerSous_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = listeTaches.Count > 0;
        }
        //Bouton terminer
        private void TerminerEtape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            int index=ListBoxInfoTacheActive.SelectedIndex;
            string textSelect=ListBoxInfoTacheActive.SelectedItem.ToString();
            
            if (ListBoxTachePlanifiees.SelectedItem != null)
            {
                int compteurTache = 0;
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                    {
                        ChercherEtChangerTerminee(tache, index);
                        textSelect += " (Terminée)";
                        ListBoxInfoTacheActive.Items.RemoveAt(index);
                        ListBoxInfoTacheActive.Items.Insert(index, textSelect);
                        SelectionnerPremierEtapeDispo(tache);
                        VerifierDateDebut(tache,compteurTache);
                        VerifierDateFin(tache, compteurTache);

                    }
                    compteurTache++;
                }
            }
            if (ListBoxTacheEnCours.SelectedItem != null)
            {
                int compteurTache = 0;
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTacheEnCours.SelectedItem))
                    {
                        ChercherEtChangerTerminee(tache, index);
                        textSelect += " (Terminée)";
                        ListBoxInfoTacheActive.Items.RemoveAt(index);
                        ListBoxInfoTacheActive.Items.Insert(index, textSelect);
                        SelectionnerPremierEtapeDispo(tache);
                        VerifierDateFin(tache, compteurTache);
                    }
                    compteurTache++;
                }
            }
        }
        
        private void TerminerEtape_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int index = ListBoxInfoTacheActive.SelectedIndex;
            if (ListBoxTachePlanifiees.SelectedItem != null)
            {
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                    {
                        e=VerifierTacheTerminee(e,tache);
                    }
                }
            }
            if (ListBoxTacheEnCours.SelectedItem != null)
            {
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTacheEnCours.SelectedItem))
                    {
                        e = VerifierTacheTerminee(e,tache);
                    }
                }
            }
        }
        //Bouton Ajouter tâche
        private void AjouterTache_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string descriptionTache = CreationNouvelleTache.Text;
            Tache nouvelleTache=new Tache(descriptionTache);
            listeTaches.Add(nouvelleTache);
            ListBoxTachePlanifiees.Items.Add(descriptionTache);
            CreationNouvelleTache.Text = "";
            ListBoxInfoTacheActive.Items.Clear();
        }
        private void AjouterTache_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!CreationNouvelleTache.Text.Equals(""))
            {
                e.CanExecute = true;
            }
            
        }
        //Bouton supprimer étape
        private void SupprimerEtape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ListBoxTachePlanifiees.SelectedItem != null)
            {
                int compteurTache = 0;
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                    {
                        supprimerUnEtape(tache, compteurTache);

                    }
                    compteurTache++;
                }
            }
            if (ListBoxTacheEnCours.SelectedItem != null)
            {
                int compteurTache = 0;
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTacheEnCours.SelectedItem))
                    {
                        supprimerUnEtape(tache, compteurTache);
                    }
                    compteurTache++;
                }
            }
        }
        private void SupprimerEtape_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            int index = ListBoxInfoTacheActive.SelectedIndex;
            if (ListBoxTachePlanifiees.SelectedItem != null)
            {
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                    {
                        e = VerifierTacheTerminee(e, tache);
                    }
                }
            }
            if (ListBoxTacheEnCours.SelectedItem != null)
            {
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTacheEnCours.SelectedItem))
                    {
                        e = VerifierTacheTerminee(e, tache);
                    }
                }
            }
        }
        //Bouton Ajouter étape
        private void AjouterEtape_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (ListBoxTachePlanifiees.SelectedItem != null)
            {
                foreach (Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                    {
                        String descriptionEtape= NomTacheActive.Text;
                        int nbEtape=1+(tache.listeEtapes.Count);
                        Etape nouvelleEtape = new Etape(descriptionEtape,nbEtape);
                        tache.listeEtapes.Add(nouvelleEtape);
                        ListBoxInfoTacheActive.Items.Add(descriptionEtape);
                        NomTacheActive.Text = "";

                    }
                }
            }
            if (ListBoxTacheEnCours.SelectedItem != null)
            {
                foreach(Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTacheEnCours.SelectedItem))
                    {
                        String descriptionEtape = NomTacheActive.Text;
                        int nbEtape = 1 + (tache.listeEtapes.Count);
                        Etape nouvelleEtape = new Etape(descriptionEtape, nbEtape);
                        tache.listeEtapes.Add(nouvelleEtape);
                        ListBoxInfoTacheActive.Items.Add(descriptionEtape);
                        NomTacheActive.Text = "";
                    }
                }
            }
            if(ListBoxTacheTerminees.SelectedItem != null)
            {
                foreach(Tache tache in listeTaches)
                {
                    if (tache.Description.Equals(ListBoxTacheTerminees.SelectedItem))
                    {
                        String descriptionEtape = NomTacheActive.Text;
                        int nbEtape = 1 + (tache.listeEtapes.Count);
                        int indexSeleted = ListBoxTacheTerminees.SelectedIndex;
                        Etape nouvelleEtape = new Etape(descriptionEtape, nbEtape);
                        tache.listeEtapes.Add(nouvelleEtape);
                        ListBoxInfoTacheActive.Items.Add(descriptionEtape);
                        NomTacheActive.Text = "";
                        tache.Fin = "";
                        DataContext = tache;
                        ListBoxTacheEnCours.Items.Add(ListBoxTacheTerminees.SelectedItem);
                        ListBoxTacheTerminees.Items.RemoveAt(indexSeleted);
                        ListBoxInfoTacheActive.Items.Clear();
                    }
                }
            }
        }
        private void AjouterEtape_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!NomTacheActive.Text.Equals(""))
            {
                e.CanExecute = true;
            }
        }
        //Selectionne listBox tâches planifiées
        private void ListBoxTachePlanifiees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach(Tache tache in listeTaches)
            {
                if (tache.Description.Equals(ListBoxTachePlanifiees.SelectedItem))
                {
                    DataContext = tache;
                    ListBoxTacheEnCours.UnselectAll();
                    ListBoxTacheTerminees.UnselectAll();
                    ListBoxInfoTacheActive.Items.Clear();
                    ChargerEtapeListBox(tache);
                    SelectionnerPremierEtapeDispo(tache);
                }
            }
        }
        //Selectionne listBox tâches en cours
        private void ListBoxTacheEnCours_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (Tache tache in listeTaches)
            {
                if (tache.Description.Equals(ListBoxTacheEnCours.SelectedItem))
                {
                    DataContext = tache;
                    ListBoxTachePlanifiees.UnselectAll();
                    ListBoxTacheTerminees.UnselectAll();
                    ListBoxInfoTacheActive.Items.Clear();
                    ChargerEtapeListBox(tache);
                    SelectionnerPremierEtapeDispo(tache);
                }
            }
        }

        //Selectionne listBox tâches terminées
        private void ListBoxTacheTerminees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            foreach (Tache tache in listeTaches)
            {
                if (tache.Description.Equals(ListBoxTacheTerminees.SelectedItem))
                {
                    DataContext = tache;
                    ListBoxTachePlanifiees.UnselectAll();
                    ListBoxTacheEnCours.UnselectAll();
                    ListBoxInfoTacheActive.Items.Clear();
                    ChargerEtapeListBox(tache);
                }
            }
        }
        //Selectionne listBox tâches planifiées
        private void ListBoxInfoTacheActive_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }
        
        //Méthodes
        private void OuvrirFichier()
        {
            string nomFichier = "";
            

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "Fichier";
            dialog.DefaultExt = ".xml";
            dialog.Filter = "xml files (.xml)|*.xml";

            bool? resultat = dialog.ShowDialog();

            if (resultat.HasValue && resultat.Value)
            {
                nomFichier = dialog.FileName;
                NomFichier = nomFichier;
            }
            ChargerTaches(nomFichier);
            ChargerTacheListBox();
        }
        private void ChargerTaches(String nomFichier)
        {
            XmlDocument document = new XmlDocument();
            document.Load(nomFichier);

            ListBoxTachePlanifiees.Items.Clear();
            ListBoxTacheEnCours.Items.Clear();
            ListBoxTacheTerminees.Items.Clear();
            ListBoxInfoTacheActive.Items.Clear();
            listeTaches =new List<Tache>();
            XmlElement racine = document.DocumentElement;
            XmlNodeList taches = racine.GetElementsByTagName("tache");
            foreach (XmlNode tache in taches)
            {
                XmlElement elem = tache as XmlElement;
                Tache newTache = new Tache(elem);
                listeTaches.Add(newTache);

            }
        }
        private void ChargerTacheListBox()
        {
            foreach (Tache unTache in listeTaches)
            {
                if (unTache.Debut.Equals("") && unTache.Fin.Equals(""))
                {
                    ListBoxTachePlanifiees.Items.Add(unTache.Description);
                }
                else if (!unTache.Debut.Equals("") && unTache.Fin.Equals(""))
                {
                    ListBoxTacheEnCours.Items.Add(unTache.Description);
                }
                else if (!unTache.Debut.Equals("") && !unTache.Fin.Equals(""))
                {
                    ListBoxTacheTerminees.Items.Add(unTache.Description);
                }
            }
        }
        private void SauvegarderTaches(string nomFichier)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement racine = doc.CreateElement("taches");
            doc.AppendChild(racine);
            foreach(Tache unTache in listeTaches)
            {
                racine.AppendChild(unTache.ToXML(doc));
            }
            doc.Save(nomFichier);
        }
        private void ChargerEtapeListBox(Tache unTache)
        {
            foreach(Etape unEtape in unTache.listeEtapes)
            {
                ListBoxInfoTacheActive.Items.Add(unEtape.Description);
            }
        }
        private void ChercherEtChangerTerminee(Tache unTache,int index)
        {
            foreach(Etape etape in unTache.listeEtapes)
            {
                if (etape.Nombre==(index+1))
                {
                    etape.Termine = true;
                    etape.VerifierTerminationEtape();
                }
            }
        }
        private CanExecuteRoutedEventArgs VerifierTacheTerminee(CanExecuteRoutedEventArgs e, Tache unTache)
        {
            foreach (Etape etape in unTache.listeEtapes)
            {
                if (etape.Description.Equals(ListBoxInfoTacheActive.SelectedItem))
                {
                    if (!etape.Termine)
                    {
                        e.CanExecute = true;
                    }
                }
            }
            return e;
        }
        private void SelectionnerPremierEtapeDispo(Tache tache)
        {
            int index=0;
            for(int i = 0; i < tache.listeEtapes.Count; i++)
            {
                if (!tache.listeEtapes[i].Termine)
                {
                    index = tache.listeEtapes[i].Nombre - 1;
                    i = tache.listeEtapes.Count;
                }
            }
            ListBoxInfoTacheActive.SelectedIndex = index;
        }
        private void VerifierDateDebut(Tache tache, int compteurTache)
        {
            if(ListBoxInfoTacheActive.Items.Count == 1)
            {
                string copieDes = ListBoxTachePlanifiees.SelectedItem.ToString();
                int compteurSeleted = ListBoxTachePlanifiees.SelectedIndex;
                ListBoxTachePlanifiees.Items.RemoveAt(compteurSeleted);
                ListBoxTacheTerminees.Items.Add(listeTaches[compteurTache].Description);
                DateTime date = DateTime.Now;
                String dateString = date.ToString("yyyy/MM/dd");
                listeTaches[compteurTache].Debut = dateString;
                listeTaches[compteurTache].Fin = dateString;
                DataContext = listeTaches[compteurTache];
                ListBoxInfoTacheActive.Items.Clear();
            }
            else
            {
                foreach (Etape etape in tache.listeEtapes)
                {
                    if (etape.Termine && listeTaches.Count != 1)
                    {
                        string copieDes = ListBoxTachePlanifiees.SelectedItem.ToString();
                        int compteurSeleted = ListBoxTachePlanifiees.SelectedIndex;
                        ListBoxTachePlanifiees.Items.RemoveAt(compteurSeleted);
                        ListBoxTacheEnCours.Items.Add(copieDes);
                        DateTime date = DateTime.Now;
                        String dateString = date.ToString("yyyy/MM/dd");
                        listeTaches[compteurTache].Debut = dateString;
                        DataContext = listeTaches[compteurTache];
                        ListBoxInfoTacheActive.Items.Clear();
                    }
                }
            }
        }
        private void VerifierDateFin(Tache tache, int compteurTache)
        {
            bool toutesEtapesTerminee=true;
            foreach(Etape etape in tache.listeEtapes)
            {
                if (!etape.Termine)
                {
                    toutesEtapesTerminee=false;
                }
            }
            if (toutesEtapesTerminee)
            {
                string copieDes = "";
                if (ListBoxTacheEnCours.SelectedItem != null)
                {
                    copieDes = ListBoxTacheEnCours.SelectedItem.ToString();
                    int compteurSeleted = ListBoxTacheEnCours.SelectedIndex;
                    ListBoxTacheEnCours.Items.RemoveAt(compteurSeleted);
                    ListBoxTacheTerminees.Items.Add(copieDes);
                    DateTime date = DateTime.Now;
                    String dateString = date.ToString("yyyy/MM/dd");
                    listeTaches[compteurTache].Fin = dateString;
                    DataContext = listeTaches[compteurTache];
                    ListBoxInfoTacheActive.Items.Clear();
                }            
            }
        }
        private void supprimerUnEtape(Tache tache, int compteurTache)
        {
            bool toutesEtapesTerminee = true;
            int index = 0;
            foreach (Etape etape in tache.listeEtapes)
            {
                if (etape.Description.Equals(ListBoxInfoTacheActive.SelectedItem))
                {
                    index=ListBoxInfoTacheActive.SelectedIndex;
                    ListBoxInfoTacheActive.Items.RemoveAt(index);
                    tache.ChangerNombreItem(index);
                }

            }
            tache.listeEtapes.RemoveAt(index);
            foreach (Etape etape in tache.listeEtapes)
            {
                if (!etape.Termine)
                {
                    toutesEtapesTerminee = false;
                }
            }
                if (toutesEtapesTerminee)
            {
                string copieDes = ListBoxTacheEnCours.SelectedItem.ToString();
                int compteurSeleted = ListBoxTacheEnCours.SelectedIndex;
                ListBoxTacheEnCours.Items.RemoveAt(compteurSeleted);
                ListBoxTacheTerminees.Items.Add(copieDes);
                DateTime date = DateTime.Now;
                String dateString = date.ToString("yyyy/MM/dd");
                listeTaches[compteurTache - 1].Fin = dateString;
                DataContext = listeTaches[compteurTache];
                ListBoxInfoTacheActive.Items.Clear();
            }
        }
    }
}
