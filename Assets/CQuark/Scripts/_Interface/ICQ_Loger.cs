using System;
using System.Collections.Generic;
using System.Text;
namespace CQuark
{
   
    public interface ICQ_Logger
    {
        void Log(string str);
        void Log_Warn(string str);
        void Log_Error(string str);
    }

}