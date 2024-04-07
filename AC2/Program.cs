using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AC2
{
    class Program
    {   
        static void Main(string[] args)
        {
            const string CSVPath = "../../../Consum.csv";
            const string XMLPath = "../../../Consum.xml";

            const string MsgMenu = "\nMenú:\n1. Comarques amb població superior a 200,000\n" +
                                          "2. Consum domèstic mitjà per comarca\n" +
                                          "3. Comarques amb consum domèstic per càpita més alt\n" +
                                          "4. Comarques amb consum domèstic per càpita més baix\n" +
                                          "5. Filtrar comarques per codi\n" +
                                          "6. Filtrar comarques per nom\n" +
                                          "7. Sortir\n\nEscull una opció: ";
            const string InvalidOptionMessage = "Opció no vàlida. Si us plau, selecciona una opció del menú.";
            const string EnterCodeMessage = "Introdueix el codi de la comarca: ";
            const string EnterNameMessage = "Introdueix el nom de la comarca: ";


            const string MsgPoblationGreaterThan200000 = "Comarques amb població superior a 200,000:";
            const string MsgMaxConsumption = "Comarques amb consum domèstic per càpita més alt:";
            const string MsgMinConsumption = "Comarques amb consum domèstic per càpita més baix:";
            const string MsgFilterByCode = "Comarques filtrades pel codi:";
            const string MsgFilterByName = "Comarques filtrades pel nom:";

            const int MaxPoblation = 200000;

            string action, name;

            int code;

            bool exit = false;


            // Se convierten los datos CSV a objetos ConsumAigua
            List<ConsumAigua> consums = Helper.SaveCSV(CSVPath);

            // Se guardan los datos en un archivo XML
            XElement xmlData = Helper.ConvertToXml(consums);
            xmlData.Save(XMLPath);


            while (!exit)
            {
                Console.Write(MsgMenu);
                action = Console.ReadLine();

                switch (action)
                {
                    // Població amb més de 200000
                    case "1":
                        List<ConsumAigua> consumsMaxPoblation = Helper.XMLFindPopulationGreaterThan(MaxPoblation, XMLPath);
                        PrintConsums(MsgPoblationGreaterThan200000, consumsMaxPoblation);
                        break;

                    // Consum domèstic per càpita mitjà per comarca
                    case "2":
                        Helper.CalcAvgConsumptionPerRegion(XMLPath);
                        break;

                    // Consum domèstic per càpita més alt
                    case "3":
                        List<ConsumAigua> maxConsumsPerCapita = Helper.RegionsWithHighestConsumption(XMLPath);
                        PrintConsums(MsgMaxConsumption, maxConsumsPerCapita);
                        break;

                    // Consum domèstic per càpita més baix
                    case "4":
                        List<ConsumAigua> minConsumsPerCapita = Helper.RegionsWithLowestConsumption(XMLPath);
                        PrintConsums(MsgMinConsumption, minConsumsPerCapita);
                        break;

                    // Consums filtrats per codi
                    case "5":
                        Console.Write(EnterCodeMessage);
                        code = Convert.ToInt32(Console.ReadLine());
                        List<ConsumAigua> consumPerCodi = Helper.FilterComarquesByCode(XMLPath, code);
                        PrintConsums(MsgFilterByCode, consumPerCodi);
                        break;

                    // Consums filtrats per nom
                    case "6":
                        Console.Write(EnterNameMessage);
                        name = Console.ReadLine();
                        List<ConsumAigua> consumPerNom = Helper.FilterComarquesByName(XMLPath, name);
                        PrintConsums(MsgFilterByName, consumPerNom);
                        break;

                    // Sortir del menú
                    case "7":
                        exit = true;
                        break;

                    // Entrada no vàlida
                    default:
                        Console.WriteLine(InvalidOptionMessage);
                        break;
                }
            }
        }

        static void PrintConsums(string message, List<ConsumAigua> consums)
        {
            Console.WriteLine(message);
            foreach (var consum in consums)
            {
                Console.WriteLine(consum);
            }
        }
    }
}