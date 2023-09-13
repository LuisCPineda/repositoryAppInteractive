using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3GP_TP1
{
    internal class Zone
    {
        private int _dimensionX;
        private int _dimensionY;

        public Zone(int dimensionX, int dimensionY)
        {
            if(dimensionX > 0 && dimensionY > 0)
            {
                this.dimensionX = dimensionX;
                this.dimensionY = dimensionY;
            } else
            {
                Console.WriteLine("Un valuer pour la création de la zone est negatif");
            }
            
        }

        public int dimensionX
        {
            
            private set
            {
                if (value < 0) { 
                    throw new ArgumentOutOfRangeException("Le valeur x est negatif"); 
                }
                _dimensionX = value;
            }
            get=> _dimensionX;
        }
        public int dimensionY
        {
            private set
            {
                if (value < 0){
                    throw new ArgumentOutOfRangeException("Le valeur y est negatif");
                }
                _dimensionY = value;
            }
            get => _dimensionY;
        }
        

    }
}
