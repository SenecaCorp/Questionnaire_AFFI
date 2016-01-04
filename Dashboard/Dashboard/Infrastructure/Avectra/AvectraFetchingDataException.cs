using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serviceutils
{
    class AvectraFetchingDataException : System.Exception
    {
        public string ErrorDescription { get; set; }

        public AvectraFetchingDataException(string _errorDescription)
        {
            ErrorDescription = _errorDescription;
        }
    }
}
