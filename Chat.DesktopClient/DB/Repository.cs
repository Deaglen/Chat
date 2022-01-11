using System.Collections.Generic;


namespace Chat.DesktopClient2.DB
{
    interface Repository
    {
        void Save(Data data);

        List<Data>GetDatas();
    }
}
