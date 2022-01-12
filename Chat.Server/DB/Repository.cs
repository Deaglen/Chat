using System.Collections.Generic;


namespace Chat.DB
{
    interface Repository
    {
        void Save(Data data);

        List<Data> GetDatas();
    }
}
