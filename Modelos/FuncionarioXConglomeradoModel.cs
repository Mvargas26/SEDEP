using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class FuncionarioXConglomeradoModel
    {
        public int IdFuncXConglo { get; set; }
        public required string IdFuncionario { get; set; }
        public int IdConglomerado { get; set; }
    }//fin class
}//fin space
