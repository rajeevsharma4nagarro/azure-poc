using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCD.MessageBus
{
    public class EmailQueue
    {
        public string ConnectionString { get; set; }
        public string Name
        {
            get; set;
        }
    }
}