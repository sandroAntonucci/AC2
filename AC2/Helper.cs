using CsvHelper;
using System.Globalization;
using System.Xml.Linq;


namespace AC2
{
	public static class Helper
	{

        // Crea una llista d'objectes consumAigua des de l'arxiu CSV
		public static List<ConsumAigua> SaveCSV(string path)
		{

            using (var reader = new StreamReader(path))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<ConsumAiguaMap>();
                    var records = csv.GetRecords<ConsumAigua>().ToList();
                    return records;
                }
            }

        }

        // Retorna un XElement que conté les dades de la llista d'objectes consumAigues
        public static XElement ConvertToXml(List<ConsumAigua> consumAigues)
        {
            XElement xmlData = new XElement("Consums",
                from consumAigua in consumAigues
                select new XElement("Consum",
                    new XElement("Any", consumAigua.Any),
                    new XElement("CodiComarca", consumAigua.CodiComarca),
                    new XElement("Comarca", consumAigua.Comarca),
                    new XElement("Poblacio", consumAigua.Poblacio),
                    new XElement("DomXarxa", consumAigua.DomXarxa),
                    new XElement("ActEcon", consumAigua.ActEcon),
                    new XElement("Total", consumAigua.Total),
                    new XElement("ConsDomPerCapita", consumAigua.ConsDomPerCapita)
                )
            );
            return xmlData;
        }

        // Identifica les comarques amb una població superior a 200000
        public static List<ConsumAigua> XMLFindPopulationGreaterThan(int maxPoblation, string path)
        {

            XDocument xmlData = XDocument.Load(path);

            var comarques = (from x in xmlData.Descendants("Consum")
                            where (int)x.Element("Poblacio") > maxPoblation
                            select x).ToList();

            return ConvertToClassConsum(comarques);

        }

        public static void PrintRegions(List<XElement> comarques)
        {
            foreach (var comarca in comarques)
            {
                Console.WriteLine("Hola");
            }
        }

        public static List<ConsumAigua> ConvertToClassConsum(List<XElement> comarques)
        {
            List<ConsumAigua> consums = new List<ConsumAigua>();
            foreach (var comarca in comarques)
            {
                ConsumAigua consum = new ConsumAigua
                {
                    Any = (int)comarca.Element("Any"),
                    CodiComarca = (int)comarca.Element("CodiComarca"),
                    Comarca = (string)comarca.Element("Comarca"),
                    Poblacio = (int)comarca.Element("Poblacio"),
                    DomXarxa = (int)comarca.Element("DomXarxa"),
                    ActEcon = (int)comarca.Element("ActEcon"),
                    Total = (int)comarca.Element("Total"),
                    ConsDomPerCapita = (float)comarca.Element("ConsDomPerCapita")
                };
                consums.Add(consum);
            }
            return consums;
        }

    }
}

