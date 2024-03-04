using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisData
{
    public class ATPPlayer
    {
        public string Code1;
        public string Code2;
        public string Name;

        public ATPPlayer(string code1, string code2, string name)
        {
            Code1 = code1;
            Code2 = code2;
            Name = name;
        }

        public ATPPlayer()
        {

        }
    }
}
