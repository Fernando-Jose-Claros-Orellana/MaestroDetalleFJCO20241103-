using System;
using System.Collections.Generic;

namespace MaestroDetalleFJCO20241103.Models
{
    public partial class Computadora
    {
        public Computadora()
        {
            Componentes = new List<Componente>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Marca { get; set; }
        public decimal? Precio { get; set; }

        public virtual IList<Componente> Componentes { get; set; }
    }
}
