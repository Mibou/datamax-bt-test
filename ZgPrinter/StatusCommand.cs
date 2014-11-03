using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ZgPrinter
{
    /// <summary>
    /// 
    /// </summary>
    class StatusCommand : PrinterCommand
    {
        public StatusCommand()
        {
            init_schema();
        }

        private void init_schema()
        {
            // Print schema
            this.schema = status;
        }
    }
}
