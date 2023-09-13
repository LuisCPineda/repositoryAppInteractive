using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3GP_TP1
{
    internal class Point
    {
        private Zone zone
        {
            get;
        }
        Random random = new Random();

        public Point()
        {
            zone = new Zone(50, 50);
        }
        
        public Coordonnee CreerCoordonneeAleatoire()
        {
            
            int xCreation = random.Next(zone.dimensionX);
            int yCreation = random.Next(zone.dimensionY);
            Coordonnee nouveauCoordonnee = new Coordonnee(xCreation, yCreation);
            return nouveauCoordonnee;
        }
    }
}
