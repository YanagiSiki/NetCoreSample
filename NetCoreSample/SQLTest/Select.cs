using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLTool
{
    public static class Select
    {
        public static SelectObj Column()
        {
            var selectObj = new SelectObj();
            return selectObj;
        }
        public static SelectObj Where(this SelectObj selectObj)
        {
            return selectObj;
        }
        public static string Output(this SelectObj selectObj)
        {
            return "string";
        }
    }
}