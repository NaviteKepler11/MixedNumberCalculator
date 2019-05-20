namespace ravin.cal.tdd
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    [TestFixture]
    public class tdd
    {
        [Test]
        public void Make_Calcultaor_Simple_Formula()
        {
            Stack<int> p = new Stack<int>();
            Stack<int> cp = new Stack<int>();
            Stack<char> o = new Stack<char>();
            Stack<char> co = new Stack<char>();

            p.Push(5);
            p.Push(10);

            o.Push('+');

            Func<int, int, char, int> calculator = (a, b, c) =>
            {
                int ret = 0;
                switch (c)
                {
                    case '+': ret = a + b; break;
                    default: break; // nop
                }
                return ret;
            };
            Assert.AreEqual(15, calculator(p.Pop(), p.Pop(), o.Pop()) );
        }

    }
}
