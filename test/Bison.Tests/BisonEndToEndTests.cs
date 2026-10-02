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
        //Arrange
        var args = new string[]{"observe", "Mikkel skider", "Taastrup"};

        using (StringWriter sw = new StringWriter())
        {
            TextWriter originalOutput = Console.Out;
            Console.SetOut(sw);

            //Act
            try
            {
                var exitCode = await Program.Main(args);
                
                //Assert
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
        //Arrange
        var args = new string[]{"read"};
        using (StringWriter sw = new StringWriter())
        {
            
            TextWriter originalOutput = Console.Out;

            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                
                //Act
                var exitCode = await Program.Main(args);
                
                //Assert
                Assert.Contains("Cheeps:", sw.ToString()); 
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

        }
    }

    [Fact]
    public async Task UnknownCommand()
    {
        // Arrange
        var args = new string[]{"reed"}; // Porposely spelled wrong.
        
        using (StringWriter sw = new StringWriter())
        {
            
            TextWriter originalOutput = Console.Out;
            Console.SetOut(sw);
            Console.SetError(sw);

            try
            {
                //Act
                var exitCode = await Program.Main(args);
                
                //Assert
                Assert.Contains("Required command was not provided.", sw.ToString()); 
            }
            finally
            {
                Console.SetOut(originalOutput);
                Console.SetError(originalOutput);
            }
        }
    }


} 