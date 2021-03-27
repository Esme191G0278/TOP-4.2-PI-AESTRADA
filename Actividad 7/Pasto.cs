using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actividad_7
{
    public class Pasto : INotifyPropertyChanged
    {

        private int altura;
        public int Altura
        {
            set { altura = value; }
            get { return altura; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Costo()
        {
            
        }
    }
}
