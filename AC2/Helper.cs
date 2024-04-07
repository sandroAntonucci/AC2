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

        // Identifica les comarques amb una població superior a maxPoblation
        public static List<ConsumAigua> XMLFindPopulationGreaterThan(int maxPoblation, string path)
        {

            XDocument xmlData = XDocument.Load(path);

            var comarques = (from x in xmlData.Descendants("Consum")
                            where (int)x.Element("Poblacio") > maxPoblation
                            select x).ToList();

            return ConvertToClassConsum(comarques);

        }

        // Converteix una llista XElement en ConsumAigua
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

        // Mostra per consola el consum mitjà per comarca
        public static void CalcAvgConsumptionPerRegion(string path)
        {
            XDocument xmlData = XDocument.Load(path);

            // Agrupa els consums per comarca i calcula la mitja de consum domèstic
            var consumsPerComarca = xmlData.Root.Elements("Consum")
                .GroupBy(e => (string)e.Element("Comarca"))
                .Select(g => new
                {
                    Comarca = g.Key,
                    ConsumMitja = g.Average(e => (float)e.Element("ConsDomPerCapita"))
                })
                .ToList();

            // Mostra les dades per consola
            foreach (var item in consumsPerComarca)
            {
                Console.WriteLine("Comarca: " + item.Comarca + " Consum mitjà: " + item.ConsumMitja);
            }
        }

        // Retorna les 10 regions amb major consumició per càpita
        public static List<ConsumAigua> RegionsWithHighestConsumption(string path)
        {
            XDocument xmlData = XDocument.Load(path);

            var comarques = xmlData.Root.Elements("Consum")
                .Select(e => new ConsumAigua
                {
                    Comarca = (string)e.Element("Comarca"),
                    Poblacio = (int)e.Element("Poblacio"),
                    Any = (int)e.Element("Any"),
                    CodiComarca = (int)e.Element("CodiComarca"),
                    DomXarxa = (int)e.Element("DomXarxa"),
                    ActEcon = (int)e.Element("ActEcon"),
                    Total = (int)e.Element("Total"),
                    ConsDomPerCapita = (float)e.Element("ConsDomPerCapita")
                })
                .OrderByDescending(c => c.ConsDomPerCapita)
                .Take(10)
                .ToList();

            return comarques;
        }

        // Retorna les 10 regions amb menor consumició per càpita
        public static List<ConsumAigua> RegionsWithLowestConsumption(string path)
        {
            XDocument xmlData = XDocument.Load(path);

            var comarques = xmlData.Root.Elements("Consum")
                .Select(e => new ConsumAigua
                {
                    Comarca = (string)e.Element("Comarca"),
                    Poblacio = (int)e.Element("Poblacio"),
                    Any = (int)e.Element("Any"),
                    CodiComarca = (int)e.Element("CodiComarca"),
                    DomXarxa = (int)e.Element("DomXarxa"),
                    ActEcon = (int)e.Element("ActEcon"),
                    Total = (int)e.Element("Total"),
                    ConsDomPerCapita = (float)e.Element("ConsDomPerCapita")
                })
                .OrderBy(c => c.ConsDomPerCapita)
                .Take(10)
                .ToList();

            return comarques;
        }

        // Filtra les comarques per nom
        public static List<ConsumAigua> FilterComarquesByName(string path, string nomComarca)
        {
            XDocument xmlData = XDocument.Load(path);
            return FilterComarques(xmlData, e => (string)e.Element("Comarca") == nomComarca);
        }

        // Filtra les comarques per codi
        public static List<ConsumAigua> FilterComarquesByCode(string path, int codiComarca)
        {
            XDocument xmlData = XDocument.Load(path);
            return FilterComarques(xmlData, e => (int)e.Element("CodiComarca") == codiComarca);
        }

        // Filtra les comarques mitjançant un predicta
        public static List<ConsumAigua> FilterComarques(XDocument xmlData, Func<XElement, bool> predicate)
        {
            var comarques = xmlData.Root.Elements("Consum")
                .Where(predicate)
                .Select(e => new ConsumAigua
                {
                    Any = (int)e.Element("Any"),
                    CodiComarca = (int)e.Element("CodiComarca"),
                    Comarca = (string)e.Element("Comarca"),
                    Poblacio = (int)e.Element("Poblacio"),
                    DomXarxa = (int)e.Element("DomXarxa"),
                    ActEcon = (int)e.Element("ActEcon"),
                    Total = (int)e.Element("Total"),
                    ConsDomPerCapita = (float)e.Element("ConsDomPerCapita")
                })
                .ToList();

            return comarques;
        }

    }
}

