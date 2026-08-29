using System.Net.NetworkInformation;

// I had to choose between making the files easy to find and making this path neat.
// The files are in bin\Debug\net10.0\TextFiles
const String readPath = "TextFiles\\PingList.txt";
const String writePath = "TextFiles\\Status.txt";

// StreamReader and Writer to read and write with files.
StreamReader reader = new StreamReader(readPath);
StreamWriter writer = new StreamWriter(writePath);

// I have these written into the file to separate cycles if I decide to go in and make it ping multiple times at a later point.
// It also serves as evidence the Program ran if you don't have anything in the PingList file. Hence, why I used a do-while loop.
String? address = "Pinging sites\n";
String timestamp = "Working...";

// Tells the user that the Program has actually started.
Console.WriteLine("Beginning to check site status");

// First time ever a do-while loop has been used, if my professor is to be believed.
do
{
    writer.WriteLine(address);
    writer.WriteLine(timestamp  + "\n");

    address = reader.ReadLine();

    try
    {
        Ping p = new Ping();

        // Mark the time just before sending the ping.
        // This should work fine, but the duplicate times in Status.txt make me suspicious it might not.
        // It may just be that a successful ping is that fast though.
        // If this were an official project I would use a breakpoint and debug.
        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // Send the ping.
        PingReply reply = p.Send(address);
        
        if (reply.Status == IPStatus.Success)
        {
            writer.WriteLine("Success!");
        }
        else
        {
            // Failed to reach and why. 
            // If I wanted I could replace the if with a switch statement for different types of failures, and the success ig, and print a more useful message.
            // i.e. 'Site doesn't exist'
            writer.WriteLine("Failed to reach site - " + reply.Status);
        }
    }

    catch (Exception e)
    {
        // Checks for Null Exception since that means reader has reached the end of the file and doesn't need to write the error.
        if (!(e is ArgumentNullException))
        {
            writer.WriteLine(e.Message);  
        }
            
    }
// If the length of address is 0 then the file's done being read. 
// This would mean PingList.txt would be unpleasant to read, but it would be best to modify its content with another program anyway. Security and whatnot. 
} while (address is {Length: > 0});

// If I decided to ping multiple times I would just have to loop everything above this with a Thread.Sleep(10000) before it restarts.
// I would also need to look up how to make reader restart from the top of the file. 
// Writer should just work fine.
reader.Close();
writer.Close();

// Kindly tells the user that something actually happened.
Console.WriteLine("Task completed successfully.");
// Prevent the console from closing immediately after saying it's done.
Console.ReadLine();