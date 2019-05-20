namespace ravin.cal.math.mixednumber
{
    public class MixedNumberCore : ICalculatorCore<MixedNumber>
    {
        public MixedNumber CreateNew(long w, long n, long d)
        {
            return new MixedNumber() { Whole = w, Numerator = n, Denominator = d==0?1:d };
        }
        public MixedNumber Plus(MixedNumber n, MixedNumber m)
        {
            ConvertWhole2Numerator(n);
            ConvertWhole2Numerator(m);
            return Simplify(new MixedNumber() { Whole = n.Whole + m.Whole, Numerator = n.Numerator * m.Denominator + m.Numerator * n.Denominator, Denominator = n.Denominator * m.Denominator });
        }
        public  MixedNumber Minus(MixedNumber n, MixedNumber m)
        {
            ConvertWhole2Numerator(n);
            ConvertWhole2Numerator(m);
            return  Simplify(new MixedNumber() { Whole = n.Whole - m.Whole, Numerator = n.Numerator - m.Numerator, Denominator = n.Denominator, });
        }
        public  MixedNumber Multiply(MixedNumber n, MixedNumber m)
        {
            ConvertWhole2Numerator(n);
            ConvertWhole2Numerator(m);
            return Simplify(new MixedNumber() { Whole = 0, Numerator = n.Numerator *  m.Numerator, Denominator = n.Denominator*m.Denominator });
        }
        public  MixedNumber Divide(MixedNumber n, MixedNumber m)
        {
            ConvertWhole2Numerator(n);
            ConvertWhole2Numerator(m);
            var ndeno = n.Denominator;
            var mdeno = m.Denominator;
            MultiplyByDemoninator(n,mdeno);
            MultiplyByDemoninator(m,ndeno);
            return Simplify(new MixedNumber() { Whole = 0, Numerator = n.Numerator, Denominator = m.Numerator });
        }
        public  MixedNumber Simplify(MixedNumber n)
        {
            ConvertWhole2Numerator(n);
            var gcd = GetGCD(n.Numerator, n.Denominator);
            n.Numerator = n.Numerator / gcd;
            n.Denominator = n.Denominator/ gcd;
            n.Whole = n.Numerator / n.Denominator;
            n.Numerator = n.Numerator % n.Denominator;
            n.Denominator = (n.Numerator == 0) ? 1 : n.Denominator;
            return n;
        }
        public  string Read(MixedNumber n)
        {
            return n.Numerator == 0?  $"{n.Whole}" : n.Whole == 0? $"{n.Numerator}/{n.Denominator}" : ReadAll(n);
        }

        #region protected 
        protected void ConvertWhole2Numerator(MixedNumber n)
        {
            n.Numerator += n.Whole * n.Denominator;
            n.Whole = 0;
        }

        protected void MultiplyByDemoninator(MixedNumber n, long d)
        {
            n.Denominator *= d;
            n.Numerator *= d;
        }

        //GCD = Greatest Common Divisor 
        protected long GetGCD(long p, long q)
        {
            if (p < 0) p = p * -1;
            if (q < 0) q = q * -1;

            while (true)
            {
                var rem = p % q;
                if (rem == 0) return q;
                p = q;
                q = rem;
            }
        }

        protected string ReadAll(MixedNumber n)
        {
            return $"{n.Whole}_{n.Numerator}/{n.Denominator}";
        }
        #endregion
    }
}
