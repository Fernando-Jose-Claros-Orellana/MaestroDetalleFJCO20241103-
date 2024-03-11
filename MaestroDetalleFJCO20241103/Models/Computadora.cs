using System;
using System.Collections.Generic;

namespace MaestroDetalleFJCO20241103.Models
{
    public partial class Computadora
    {
        public Computadora()
        {
            Componentes = new HashSet<Componente>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Marca { get; set; }
        public decimal? Precio { get; set; }

        public virtual ICollection<Componente> Componentes { get; set; }
    }
}
