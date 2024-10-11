using SuperSocketStudy.Network;
using System;
using System.Timers;

namespace SuperSocketStudy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new MyServer();

            // Config 로드
            if (false == server.InitConfig())
            {
                return;
            }

            // Config 및 핸들러 등록
            if(false == server.SetupServer())
            {
                return;
            }

            if (false == server.Start())
            {
                return;
            }

            Console.WriteLine("\n서버 네트워크 시작\nkey를 누르면 종료한다....");
            Console.ReadKey();
        }
    }
}
