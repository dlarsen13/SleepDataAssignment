string dir = Directory.GetCurrentDirectory();
bool running = true;
string currentLine;
string[] commaSplit;
string[] pipeSplit;
DateTime entryDate;
List<string> pipeSplitter = new List<string>();
int total;
double average;

// ask for input
while(running)
{
    Console.WriteLine("Enter 1 to create data file.");
    Console.WriteLine("Enter 2 to parse data.");
    Console.WriteLine("Enter anything else to quit.");
    // input response
    string? resp = Console.ReadLine();

    if (resp == "1")
    {
        // create file
        StreamWriter sw = new StreamWriter("data.txt");

        // ask a question
        Console.WriteLine("How many weeks of data is needed?");
        // input the response (convert to int)
        int weeks = int.Parse(Console.ReadLine());
        // determine start and end date
        DateTime today = DateTime.Now;
        // we want full weeks sunday - saturday
        DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
        // subtract # of weeks from endDate to get startDate
        DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
        Console.WriteLine(dataDate);
        // random number generator
        Random rnd = new Random();

        // loop for the desired # of weeks
        while (dataDate < dataEndDate)
        {
            // 7 days in a week
            int[] hours = new int[7];
            for (int i = 0; i < hours.Length; i++)
            {
                // generate random number of hours slept between 4-12 (inclusive)
                hours[i] = rnd.Next(4, 13);
            }
            // M/d/yyyy,#|#|#|#|#|#|#
            sw.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
            // add 1 week to date
            dataDate = dataDate.AddDays(7);
        }
        sw.Close();
    }
    else if (resp == "2")
    {
        //Checking if data,txt exists before starting a steamreader
        if (File.Exists(dir + "\\data.txt"))
        {
            StreamReader sr = new StreamReader(dir + "\\data.txt");
            do
            {
                // read in next line of file
                currentLine = sr.ReadLine();
                // split into an array by the comma, sperating date and hours slept
                commaSplit = currentLine.Split(",");
                // converting date into dateTime value and applying formatting
                entryDate = Convert.ToDateTime(commaSplit[0]);
                Console.WriteLine("Week of{0: MMM, dd, yyyy}", entryDate);
                // non-data dependant formatting that I just hardcoded in
                Console.WriteLine(" Su Mo Tu We Th Fr Sa Tot Avg");
                Console.WriteLine(" -- -- -- -- -- -- -- --- ---");
                // throwing the array into a list
                pipeSplitter.AddRange(commaSplit);
                // removing the dateTime value so the list is only the hours slept
                pipeSplitter.RemoveAt(0);
                // throwing the list back into string format
                currentLine = String.Join('|', pipeSplitter);
                // immediately splitting the data by the pipe, making an array of only hours slept
                pipeSplit = currentLine.Split("|");
                // looping through the sleep data array to display final output
                total = 0;
                foreach (string s in pipeSplit)
                {
                    // if/else statement to check the value's length and apply appropriate spacing
                    if (s.Length == 1){
                        Console.Write($"  {s}");
                    }
                    else{
                        Console.Write($" {s}");
                    }
                    total += Convert.ToInt32(s);
                }
                average = total / 7.0;
                Console.Write("  {0} {1:0.0}", total, average);
                Console.WriteLine($"\n");
                pipeSplitter.Clear();
            }while (sr.Peek() > 0);
        }
        
        else{
            Console.WriteLine("Error: No data file found");
        }
    }
    else
        running = false;
}
