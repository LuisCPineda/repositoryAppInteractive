using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3GP_TP1
{
    internal class Taxi
    {
        private static int idTaxi = 1;
        private static object VERROU_IDTAXI=new object();
        public Taxi()
        {
            lock (VERROU_IDTAXI)
            {
               IdTaxi= idTaxi++;
            }
            Point creationPoints = new Point();
            this.CoordonneesTaxi = creationPoints.CreerCoordonneeAleatoire();
            this.EtatTaxi = true;
        }
        
        public bool EtatTaxi
        {
            get;
            set;
        }
        public Coordonnee CoordonneesTaxi
        {
            get;
            set;
        }
        public int IdTaxi
        {
            private set;
            get;
        }
        public Voyage VoyageAssigne
        {
            set;
            get;
        }

        public string StringPositionTaxi()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("Le taxi ");
            chaine.Append(IdTaxi);
            chaine.Append(" est en fonction à (");
            chaine.Append(CoordonneesTaxi.x);
            chaine.Append(",");
            chaine.Append(CoordonneesTaxi.y);
            chaine.Append(")");
            return chaine.ToString();
        }
        public string StringAssignationTaxi()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("Voyage ");
            chaine.Append(VoyageAssigne.NbVoyage);
            chaine.Append(" assigné à Taxi ");
            chaine.Append(IdTaxi);
            return chaine.ToString();
        }

        public string StringTaxiArriveDepart()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("*Client du voyage ");
            chaine.Append(VoyageAssigne.NbVoyage);
            chaine.Append(" entre dans le taxi ");
            chaine.Append(IdTaxi);
            chaine.Append(" à (");
            chaine.Append(CoordonneesTaxi.x);
            chaine.Append(",");
            chaine.Append(CoordonneesTaxi.y);
            chaine.Append(")");
            return chaine.ToString();
        }

        public string StringTaxiArriveFinCours()
        {
            StringBuilder chaine = new StringBuilder();
            chaine.Append("***Client du voyage ");
            chaine.Append(VoyageAssigne.NbVoyage);
            chaine.Append(" sort dans le taxi ");
            chaine.Append(IdTaxi);
            chaine.Append(" à (");
            chaine.Append(CoordonneesTaxi.x);
            chaine.Append(",");
            chaine.Append(CoordonneesTaxi.y);
            chaine.Append(")");
            return chaine.ToString();
        }
    }
}
