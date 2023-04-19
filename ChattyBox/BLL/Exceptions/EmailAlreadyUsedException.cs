﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class EmailAlreadyUsedException : Exception
    {
        public EmailAlreadyUsedException() 
        { 
            
        }

        public EmailAlreadyUsedException(string message) : base(message)
        {
        }
    }
    
}