using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Store.Infrastructure
{
    public class Program
    {
        public class Source
        {
            public Source(int A)
            {
                this.A = A;
            }
            public int A { get; set; }
        }

        public class Destination
        {
            public Destination(int A)
            {
                A = A;
            }
            public int A { get; set; }
        }

        public enum A
        {
            C,
            D,
            E
        }
        public static void Main(string[] args)
        {

        }
    }
}
