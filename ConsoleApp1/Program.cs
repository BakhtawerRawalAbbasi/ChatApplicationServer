using Communication;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataCommunication communication = new DataCommunication();
            MessageHandler message = new MessageHandler(communication);
            communication.Run();
          
        }
    }
}
