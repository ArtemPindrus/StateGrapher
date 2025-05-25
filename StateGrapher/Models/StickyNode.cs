using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateGrapher.Models {
    public class StickyNode : Node {
        public string? Text { get; set; }

        protected override string? ValidateName(string? name) => name;
    }
}
