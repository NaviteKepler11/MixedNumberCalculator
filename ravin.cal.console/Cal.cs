namespace ravin.cal.console
{
    using System;
    using System.Linq;
    using Autofac;
    using ravin.cal.math;
    using ravin.cal.math.mixednumber;
    public class Cal
    {
        public static void Main(string[] args)
        {
            new Cal().Run();
        }

        IContainer  container ;
        public Cal() {
            container = BuildContainer();
        }
        #region

        protected void Run()
        {
            Console.WriteLine("Welcome to ravin's calculator..");
            var input = string.Empty;
            using (var scope = container.BeginLifetimeScope())
            {
                do
                {
                    Console.Write("cal>");
                    input = Console.ReadLine().Trim().ToLower();
                    switch (input)
                    {
                        case "exit":
                        case "over":
                        case "close": break;
                        case "":
                        case "help":
                            Console.WriteLine("Usage:  cal command");
                            Console.WriteLine("Commands:");
                            Console.WriteLine("\t\thelp|blank space - for help.");
                            Console.WriteLine("\t\texit|close|over - for exit.");
                            Console.WriteLine("\t\toperators - for show all operators");
                            Console.WriteLine("\t\t?{math equation}  - for calculation of math equation.");
                            break;
                        case "operators":
                             scope.Resolve<ICalculator>()
                                .GetOperators()
                                .ToList()
                                .ForEach(o => Console.WriteLine($"\t\t{o}") );
                             break;
                        default: 
                            if (!(input.Length > 1 && input[0] == '?'))
                            {
                                Console.WriteLine($"Invalid math expression.");
                                break;
                            }
                            try
                            {
                                Console.WriteLine($"={scope.Resolve<ICalculator>().Calculate(input.Substring(1))}");
                            }catch(Exception ex)
                            {
                                Console.WriteLine($"={ex.Message}");
                            }
                            break;
                    }
                } while (!(input.Equals("exit") || input.Equals("over") || input.Equals("close")));

            }
        }//Cal

        #endregion

        #region private
        IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Calculator<MixedNumber>>().As<ICalculator>().SingleInstance();
            builder.RegisterType<MixedNumberCore>().As<ICalculatorCore<MixedNumber>>().SingleInstance();
            return builder.Build();
        }

        #endregion
    }//ravin.cal.console.Cal

}//ravin.cal.console

