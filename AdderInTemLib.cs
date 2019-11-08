using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NearlyCloud_bot
{
    class AdderInTemLib
    {
        private static int num = 0;

        
     
        Timer timer = new Timer(Add, num, 0, 6000);

        public object file { private get; set; }

        private static void Add(object Simplefile)
        {
            //Логика добавления во временную библиотеку
        } 
    }
}
