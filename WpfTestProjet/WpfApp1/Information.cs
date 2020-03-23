using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Information
    {
        private String date;
        private String heure;
        private String temperature;
        public Information()
        {

        }

        public void SetDate(String chaine) { date = chaine; }
        public String GetDate() { return date; }
        public void SetHeure(String chaine) { heure = chaine; }
        public String GetHeure() { return heure; }
        public void SetTemperature(String chaine) { temperature = chaine; }
        public String getTemperature() { return temperature; }
    }
}
