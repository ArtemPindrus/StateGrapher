using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateGrapher.Models {
    public interface IConnectingNode {
        public IEnumerable<Connector> GetAllConnectors();
    }
}
