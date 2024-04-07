using System.Globalization;
using AC2;
using CsvHelper.Configuration;

public class ConsumAiguaMap : ClassMap<ConsumAigua>
{
    public ConsumAiguaMap()
    {
        Map(m => m.Any).Name("Any");
        Map(m => m.CodiComarca).Name("CodiComarca");
        Map(m => m.Comarca).Name("Comarca");
        Map(m => m.Poblacio).Name("Poblacio");
        Map(m => m.DomXarxa).Name("DomXarxa");
        Map(m => m.ActEcon).Name("ActEcon");
        Map(m => m.Total).Name("Total");
        Map(m => m.ConsDomPerCapita).Name("ConsDomPerCapita");
    }
}