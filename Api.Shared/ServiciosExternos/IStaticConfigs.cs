using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.ServiciosExternos
{
	public interface IStaticConfigs
	{
		string GetUrlAfip();
		string GetUrlSystelAfip();
	}
}
