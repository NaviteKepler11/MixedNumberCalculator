namespace ravin.cal.math.mixednumber
{
   public  class MixedNumber
    {
        public long Whole { get; set; }
        public long Numerator { get; set;}
        public long Denominator { get; set; }
        public MixedNumber()
        {
            Denominator = 1;// default value
        }
    }
}
