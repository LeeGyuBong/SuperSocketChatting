using SuperSocketClient.Object;
using SuperSocketClient.Scene;
using SuperSocketClient.Utility;
using System.Collections.Concurrent;

namespace SuperSocketClient.Manager
{
    public enum FormType
    {
        None = 0,
        Login = 1,
        Chat = 2,
    }

    public class FormManager : Singleton<FormManager>
    {
        private readonly ConcurrentDictionary<FormType, Form> __formList = new ConcurrentDictionary<FormType, Form>();

        public Client? Client { get; private set; } = new Client();

        ~FormManager()
        {
            __formList.Clear();
            Client = null;
        }

        private Form? CreateForm(FormType type)
        {
            Form? newFrom = null;

            if (__formList.ContainsKey(type))
            {
                newFrom = __formList[type];
            }
            else
            {
                switch (type)
                {
                    case FormType.Login:
                        newFrom = new LoginForm();
                        break;
                    case FormType.Chat:
                        newFrom = new ChatForm();
                        break;
                }

                if (newFrom != null)
                {
                    __formList.TryAdd(type, newFrom);
                }
            }

            return newFrom;
        }

        public void RemoveForm(FormType type)
        {
            if (__formList.ContainsKey(type))
            {
                __formList.TryRemove(type, out var form);
            }
        }

        public Form? GetForm(FormType type)
        {
            if (__formList.ContainsKey(type))
            {
                __formList.TryGetValue(type, out var form);
                return form;
            }

            return CreateForm(type);
        }
    }
}
