using System;

namespace AC2
{
	public class ConsumAigua
	{

		public int Any { get; set; }
		public int CodiComarca { get; set; }
		public string? Comarca { get; set; }
		public int Poblacio { get; set; }
		public int DomXarxa { get; set; }
		public int ActEcon { get; set; }
		public int Total { get; set; }
		public float ConsDomPerCapita { get; set; }


        public override string ToString()
        {
            return $"Any: {Any}, CodiComarca: {CodiComarca}, Comarca: {Comarca}, Poblacio: {Poblacio}, DomXarxa: {DomXarxa}, ActEcon: {ActEcon}, Total: {Total}, ConsDomPerCapita: {ConsDomPerCapita}";
        }

    }
}

