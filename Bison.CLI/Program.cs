using System;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
class Program
{
    static void Main(string[] args)
    {
        if(args[0] == "read")
        {
            read();
        }
        else if (args[0] == "observe")
        {
            observe(args[1]);
        }    
    }

    public static void read()
    {
        string[] titles = new string[3];
        List<post> posts = new List<post>();



        try
        {
            using StreamReader reader = new ("bison_observe_cli_db.csv");
            string line;
            bool firstline = true;
            while ((line = reader.ReadLine() ) != null)
            {
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                string[] X = CSVParser.Split(line);
                if(firstline)
                {
                    titles[0] = X[0];
                    titles[1] = X[1];
                    titles[2] = X[2];
                    firstline = false;
                }
                else
                {
                    posts.Add(new post(X[0],X[2],X[1]));
                }    
            }
            foreach(post p in posts)
            {
                Console.WriteLine(p.printPost());
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }
    }
  

    public static void observe(string observation)
    {
        DateTimeOffset localTime = DateTimeOffset.Now;
        long unixTime = localTime.ToUnixTimeSeconds();
        string tidspunkt = unixTime.ToString();
        
        string author = Environment.UserName;
        

        using(StreamWriter sw = File.AppendText("bison_observe_cli_db.csv"))
        {
            sw.WriteLine(author + "," + '"' + observation + '"' + "," + tidspunkt);
        }
    }


}
class post
{
    string author;
    string timestamp;
    string observation;
    public post(string author, string timestamp, string observation)
    {
        this.author = author;
        this.timestamp = timestamp;
        this.observation = observation;
    }

    private string convertTimestampNew()
    {
        DateTimeOffset timeInSeconds = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp));
        return timeInSeconds.Day + "/" + timeInSeconds.Month + "/" + timeInSeconds.Year + " " + timeInSeconds.Hour + ":" +timeInSeconds.Minute + ":" + timeInSeconds.Second;
    }

    public string printPost()
    {
        return author + " @ " + convertTimestampNew() + ": " + observation;
    }
}
