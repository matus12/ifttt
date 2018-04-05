using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IFTTT
{
    public class ContentError
    {
        public Error[] Errors;

        public ContentError()
        {
            Errors = new Error[1];
            Errors[0] = new Error();
        }
    }
}