using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3GP_TP1
{
    internal class Voyage
    {
        private static int nbVoyage=1;
        private static object VERROU_NB=new object();

        public Voyage()
        {
            Point creationPoints = new Point();
            this.coordonneeVoyageDepart = creationPoints.CreerCoordonneeAleatoire();
            this.coordonneeVoyageArrivee = creationPoints.CreerCoordonneeAleatoire();
            this.voyagePrisParTaxi = false;
            this.VoyageEffectue = false;
            this.VoyageAuPointDepart = false;
            lock (VERROU_NB)
            {
                this.NbVoyage = nbVoyage++;
            }
            
            
        }
        public Coordonnee coordonneeVoyageDepart
        {
            get;
        }
        public bool voyagePrisParTaxi 
        { 
            get; 
            set; 
        }
        public bool VoyageAuPointDepart { 
            get; 
            set; 
        }
        public bool VoyageEffectue 
        { 
            get; 
            set; 
        }
        public Coordonnee coordonneeVoyageArrivee
        {
            get;
        }

        public int NbVoyage
        {
            private set;
            get;
        }


        public string AfficherDonneeVoyage()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("Creation voyage ");
            chaine.Append(NbVoyage);
            chaine.Append(" Depart: (");
            chaine.Append(coordonneeVoyageDepart.x);
            chaine.Append(",");
            chaine.Append(coordonneeVoyageDepart.y);
            chaine.Append(") Arrivée: (");
            chaine.Append(coordonneeVoyageArrivee.x);
            chaine.Append(",");
            chaine.Append(coordonneeVoyageArrivee.y);
            chaine.Append(")");
            return chaine.ToString();
        }
        public string AfficherCoordonneeDepart()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("(");
            chaine.Append(coordonneeVoyageDepart.x);
            chaine.Append(",");
            chaine.Append(coordonneeVoyageDepart.y);
            chaine.Append(")");
            return chaine.ToString();
        }

        public string AfficherCoordonneArrivee()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("(");
            chaine.Append(coordonneeVoyageArrivee.x);
            chaine.Append(",");
            chaine.Append(coordonneeVoyageArrivee.y);
            chaine.Append(")");
            return chaine.ToString();
        }

    }
}
