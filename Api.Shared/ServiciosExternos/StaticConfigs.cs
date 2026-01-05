using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.ServiciosExternos
{
	public class StaticConfigs: IStaticConfigs
	{
	
		private string Urlafip { get; set; } = @"https://recetas.instantagro.goodapps.com.ar/GetContribuyente.ashx?CUIT=";
		private string UrlSystelFrigo { get; set; } = @"http://goodappssystell.ddns.net/api/";


	

		public string GetUrlAfip()
		{
			return Urlafip;
		}
        public string GetUrlSystelAfip()
        {
            return UrlSystelFrigo;
        }
    }
}
