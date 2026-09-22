namespace DefaultNamespace;
using Spectre.Console;
using Xunit;
using Bison;
using SimpleDB;
 
public class BisonEndToEndTests
{   
    [Fact]
    public async Task AddObservations()
    {
        var args = new string[]{"observe", "Mikkel skider", "Taastrup"};

        using (StringWriter sw = new StringWriter())
        {
            
            TextWriter originalOutput = Console.Out;

            Console.SetOut(sw);

            try
            {
                var exitCode = await Program.Main(args);
                Assert.Contains("Successfully added observation", sw.ToString());
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

        }
    }

    [Fact]
    public async Task observeObservations()
    {
        var args = new string[]{"read"};
        using (StringWriter sw = new StringWriter())
        {
            
            TextWriter originalOutput = Console.Out;

            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                var exitCode = await Program.Main(args);
                Assert.Contains("Cheeps:", sw.ToString()); 
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

        }
    }
    
    
} 