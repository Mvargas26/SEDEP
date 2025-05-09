﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class FuncionarioViewModel
    {
        public FuncionarioModel Funcionario { get; set; }
        public List<PuestoModel> Puestos { get; set; }
        public List<ConglomeradoModel> Conglomerados { get; set; }
        public List<DepartamentoModel> Departamentos { get; set; }
        public List<RolesModel> Roles { get; set; }
        public List<EstadoFuncionarioModel> EstadosFuncionario { get; set; }
    }
}
