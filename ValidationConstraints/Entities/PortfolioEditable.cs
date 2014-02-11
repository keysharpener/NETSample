using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationConstraintsObjectModel.Entities
{
    public class PortfolioEditable
    {
        public string Comment { get; set; }
        public UserInfo Manager { get; set; }
    }
}
