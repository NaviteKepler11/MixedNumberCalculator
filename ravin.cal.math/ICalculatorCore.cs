namespace ravin.cal.math
{
    public interface ICalculatorCore<T>
    {
        T CreateNew(long w, long n, long d);
        T Plus(T n, T m);
        T Minus(T n, T m);
        T Multiply(T n, T m);
        T Divide(T n, T m);
        T Simplify(T n);
        string Read(T n);
    }
}
