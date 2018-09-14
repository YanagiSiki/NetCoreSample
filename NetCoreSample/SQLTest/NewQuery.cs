using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLTool
{
    public class NewQuery
    {
        public string GetSQLString()
        {       
            return Select.Column().Where().Where().Output();
        }
    }
}