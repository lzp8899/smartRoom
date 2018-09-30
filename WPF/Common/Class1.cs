using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Web.Common
{
    class Class1
    {
        public String getBaseAn()
        {
            Random r1 = new Random(32);
            Random r2 = new Random(10);

            return r1.ToString()+"."+r2.ToString();
        }

        public String get5To10(int kl)
        {
            return getBaseAn()+kl*1.12;
        }

        public String get10To23(int kl)
        {
            ThreadStaticAttribute thread = new ThreadStaticAttribute();
            return getBaseAn()+kl*1.12;
        }

        public String get23To5()
        {
            Random r = new Random(10);
            return getBaseAn();
        }
    }
}
