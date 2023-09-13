using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3GP_TP1
{
    internal class Coordonnee
    {
       
        public Coordonnee(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public int x
        {
            set;
            get;
        }
        public int y
        {
            get;
            set;
        }
    }
}
