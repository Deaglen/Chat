using System.Collections.Generic;


namespace Chat.DesktopClient2.DB
{
    interface IRepository<T>
        where T : class
    {
        void Save(T data);

        List<T> GetDatas();
    }
}
