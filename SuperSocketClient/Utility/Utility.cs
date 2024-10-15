namespace SuperSocketClient.Utility
{
    public class Singleton<T> where T : class, new()
    {
        private static volatile T? __instance = null;
        private static object __lock = new object();

        public static T Instance
        {
            get
            {
                if (__instance == null)
                {
                    lock (__lock)
                    {
                        if (__instance == null)
                        {
                            __instance = new T();
                        }
                    }
                }
                return __instance;
            }
        }
    }
}
