using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crossie.Model
{
    class ExceptionHandeling : Exception
    {
        public ExceptionHandeling()
        {
            ExceptionError();
        }

        public string ExceptionError()
        {
            return "Fatal error in system file... System will now save and exit..";
        }
    }
}
