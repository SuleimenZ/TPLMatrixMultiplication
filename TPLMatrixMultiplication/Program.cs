using MathNet.Numerics.LinearAlgebra;
using System.Diagnostics;


//Collected data: https://docs.google.com/spreadsheets/d/1orGI0EqUYCASaU1_UFMFEGOevXNRzeUTL3eMoKipI_0/edit?usp=sharing
//Commented code changes culture, so that fraction is separated with dot (.), rather than coma (,). Helps with filling spreadsheets
//System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
//customCulture.NumberFormat.NumberDecimalSeparator = ".";

//System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

Stopwatch sw = new();

double mathNetTime, serialForTime, serialWhileTime, parallelForTime, parallelWhileTime;

var system = new TPLMatrixMultiplication.TPLMatrixMultiplication(500,500,500);

system.RandomizeMatricies();

Console.WriteLine("Ignore results of #0, as they usually take longer time.");
int i = 0;
while(i < 11)
{
    Console.WriteLine($"////////// #{i} //////////");

    //MathNet, used for verifying result
    sw.Restart();
    system.MathNetMultiplication();
    sw.Stop();
    mathNetTime = sw.Elapsed.TotalMilliseconds;
    var mathNetResult = system.ResultMatrix;
    Console.WriteLine("MathNet: " + mathNetTime);

    //SerialFor
    sw.Restart();
    system.SerialForMultiplication();
    sw.Stop();
    serialForTime = sw.Elapsed.TotalMilliseconds;
    var serialForResult = system.ResultMatrix;
    Console.WriteLine("Serial for: " + serialForTime + " " + mathNetResult.Equals(serialForResult));

    //SerialWhile
    sw.Restart();
    system.SerialWhileMultiplication();
    sw.Stop();
    serialWhileTime = sw.Elapsed.TotalMilliseconds;
    var serialWhileResult = system.ResultMatrix;
    Console.WriteLine("Serial while: " + serialWhileTime + " " + mathNetResult.Equals(serialWhileResult));

    //ParallelFor
    sw.Restart();
    system.ParallelForMultiplication(8);
    sw.Stop();
    parallelForTime = sw.Elapsed.TotalMilliseconds;
    var parallelForResult = system.ResultMatrix;
    Console.WriteLine("Parallel for: " + parallelForTime + " " + mathNetResult.Equals(parallelForResult));

    //ParallelWhile
    sw.Restart();
    system.ParallelWhileMultiplication(8);
    sw.Stop();
    parallelWhileTime = sw.Elapsed.TotalMilliseconds;
    var parallelWhileResult = system.ResultMatrix;
    Console.WriteLine("Parallel while: " + parallelWhileTime + " " + mathNetResult.Equals(parallelWhileResult));
    i++;
}