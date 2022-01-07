using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeTai
{
    public class FormVe
    {
        private static DatVe ticket = new DatVe();

        public static DatVe Ticket { get => ticket; set => ticket = value; }
    }
}
