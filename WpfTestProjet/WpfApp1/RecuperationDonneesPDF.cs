using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toxy;

namespace WpfApp1
{
    class RecuperationDonneesPDF
    {

        Information information;
        ArrayList arrayInformation;
        ArrayList arrayExtraction;
        public RecuperationDonneesPDF()
        {
            information = new Information();
            arrayInformation = new ArrayList();
            arrayExtraction = new ArrayList();
        }
        /*public static IEnumerable<string> GetPdfFiles()
        {
            return Directory.GetFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "*.pdf", SearchOption.AllDirectories);
        }*/

        public ArrayList Processus()
        {

            ArrayList arrayTemp = new ArrayList();
            ArrayList arrayTemperature = new ArrayList();

            /*foreach (var pdfFileName in GetPdfFiles())
            {*/
               
                //extraction d'information du pdf
                Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "Pdf Files|*.pdf";
            fileDialog.ShowDialog();
                String pdf = fileDialog.FileName;

                StreamWriter sw = new StreamWriter(pdf + ".txt");
                Console.WriteLine(pdf);
                Console.WriteLine("----------------------------------");
                var parser = new Toxy.Parsers.PDFTextParser(new ParserContext(pdf));
                
                    string result = parser.Parse();
                    sw.WriteLine(result);
                    sw.Close();
                
                
                
                StreamReader sr = new StreamReader(pdf + ".txt");
                String ligne;
                int conteur = 0;
                do
                {
                    ligne = sr.ReadLine();
                    if (ligne.Contains("Start Time"))
                    {
                        sr.ReadLine();
                        if (conteur != 0)
                        {
                            do
                            {
                                ligne = sr.ReadLine();
                                if (!ligne.Substring(0, 3).Equals("www"))
                                {
                                    arrayExtraction.Add(ligne);

                                }
                            } while (!ligne.Substring(0, 3).Equals("www"));
                        }
                        conteur = conteur + 1;

                    }
                } while (!sr.EndOfStream);
            //}
            String coupe;
            //separation de les differentes informations
            for (int i = 0; i < arrayExtraction.Count; i++)
            {
                int position = 1;
                coupe = arrayExtraction[i].ToString();

                //faire le meme action une certain fois dependant de la longueur du ligne
                if (coupe.Length > 150)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        position = extractionInformationDuString(position, coupe);

                    }
                }
                else if (coupe.Length > 130)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        position = extractionInformationDuString(position, coupe);
                    }
                }
                else if (coupe.Length > 100)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        position = extractionInformationDuString(position, coupe);
                    }
                }
                else if (coupe.Length > 70)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        position = extractionInformationDuString(position, coupe);
                    }
                }
                else if (coupe.Length > 35)
                {
                    position = extractionInformationDuString(position, coupe);
                    position = extractionInformationDuString(position, coupe);
                }
                else
                {
                    position = extractionInformationDuString(position, coupe);
                }

            }
            Console.WriteLine(arrayInformation);
            //tri à bulle pour la date
            for (int i = 0; i < arrayInformation.Count - 1; i++)
            {
                
                for (int j = 0; j < arrayInformation.Count - i - 1; j++)
                {
                    Information premierInformation = (Information)arrayInformation[j];
                    int premierDate = int.Parse(premierInformation.GetDate().Substring(0, 2));
                    Information deuxiemInformation = (Information)arrayInformation[j + 1];
                    int deuxiemDate = int.Parse(deuxiemInformation.GetDate().Substring(0, 2));
                    if (premierInformation.GetDate().Substring(3, 6) != deuxiemInformation.GetDate().Substring(3, 6))
                    {
                        if ((premierDate < deuxiemDate))
                        {
                            Information temporaire = (Information)arrayInformation[j];
                            arrayInformation[j] = arrayInformation[j + 1];
                            arrayInformation[j + 1] = temporaire;
                        }
                    }
                    else if ((premierDate > deuxiemDate))
                    {
                        Information temporaire = (Information)arrayInformation[j];
                        arrayInformation[j] = arrayInformation[j + 1];
                        arrayInformation[j + 1] = temporaire;
                    }
                }
            }
            //tri à bulle pour le heure
            for (int i = 0; i < arrayInformation.Count - 1; i++)
            {
                for (int j = 0; j < arrayInformation.Count - i - 1; j++)
                {
                    Information premierInformation = (Information)arrayInformation[j];
                    Information deuxiemInformation = (Information)arrayInformation[j + 1];
                    int premierHeure = int.Parse(premierInformation.GetHeure().Substring(0, 2));
                    int deuxiemHeure = int.Parse(deuxiemInformation.GetHeure().Substring(0, 2));
                    if ((premierInformation.GetDate() == deuxiemInformation.GetDate()) & (premierHeure > deuxiemHeure))
                    {
                        Information temporaire = (Information)arrayInformation[j];
                        arrayInformation[j] = arrayInformation[j + 1];
                        arrayInformation[j + 1] = temporaire;
                    }
                }

            }
            return (arrayInformation);
        }
        //mettre l'information du string dans le classe information et apres le met dans l'array
        public int extractionInformationDuString(int position, String coupe)
        {

            information = new Information();
            information.SetDate(coupe.Substring(position, 9));
            position = position + 10;
            information.SetHeure(coupe.Substring(position, 8));
            position = position + 8;
            int pointer = coupe.Substring(position + 3).IndexOf('C');
            information.SetTemperature(coupe.Substring(position + 3, pointer - 2));
            arrayInformation.Add(information);
            position = position + pointer + 5;
            return position;
        }

    }
}
