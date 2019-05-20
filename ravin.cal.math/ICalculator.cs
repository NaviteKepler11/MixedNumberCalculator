namespace ravin.cal.math
{
    using System.Collections.Generic;
    public interface ICalculator
    {
        IEnumerable<string> GetOperators();

        string Calculate(string input);
    }
}
