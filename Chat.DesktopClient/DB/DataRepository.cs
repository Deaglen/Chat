using System.Collections.Generic;
using System.Linq;


namespace Chat.DesktopClient2.DB
{
    class DataRepository : Repository
    {

        ApplicationContext DB;
        public DataRepository()
        {
            ApplicationContext db = new ApplicationContext();
            DB = db;
        }

        public List<Data> GetDatas()
        {
            return DB.Messeges.ToList();

        }

        public void Save(Data data)
        {
            DB.Messeges.Add(data);
            DB.SaveChanges();

        }
    }
}
