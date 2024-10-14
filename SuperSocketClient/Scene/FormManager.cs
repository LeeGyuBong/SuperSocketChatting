using SuperSocketClient.Object;
using SuperSocketClient.Utility;

namespace SuperSocketClient.Scene
{
    public enum FormType
    {
        None = 0,
        Login = 1,
        Chat = 2,
    }

    public class FormManager : Singleton<FormManager>
    {
        private readonly Dictionary<FormType, Form> __formList = new Dictionary<FormType, Form>();

        public Client Client { get; private set; } = new Client();

        ~FormManager()
        {
            __formList.Clear();
            Client = null;
        }

        private Form CreateForm(FormType type)
        {
            Form newFrom = null;

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
                    __formList.Add(type, newFrom);
                }
            }

            return newFrom;
        }

        public void RemoveForm(FormType type)
        {
            if (__formList.ContainsKey(type))
            {
                __formList.Remove(type);
            }
        }

        public Form GetForm(FormType type)
        {
            if (__formList.ContainsKey(type))
            {
                return __formList[type];
            }

            return CreateForm(type);
        }
    }
}
