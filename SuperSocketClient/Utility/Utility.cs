using SuperSocketClient.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocketClient.Utility
{
    public class Singleton<T> where T : class, new()
    {
        static volatile T __instance;
        static object __lock = new object();

        public static T Instance()
        {
            lock (__lock)
            {
                if (__instance == null)
                {
                    __instance = new T();
                }
                return __instance;
            }
        }
    }
}
