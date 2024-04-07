using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AC2
{

    class Program
    {

        public static void Main()
        {

            const string CSVPath = "../../../Consum.csv";
            const string XMLPath = "../../../Consum.xml";

            int length = 0;

            const int MaxPoblation = 200000;

            // Es converteixen les dades csv a objectes ConsumAigua
            List<ConsumAigua> consums = Helper.SaveCSV(CSVPath);

            // Es guarden les dades en un arxiu xml
            XElement xmlData = Helper.ConvertToXml(consums);
            xmlData.Save(XMLPath);

            //Ex1
            List<ConsumAigua> consumsMaxPoblation = Helper.XMLFindPopulationGreaterThan(MaxPoblation, XMLPath);

            foreach(var consumAigua in consumsMaxPoblation)
            {
                length++;
                Console.WriteLine(consumAigua);
            }
            Console.WriteLine(length);



        }

    }


}