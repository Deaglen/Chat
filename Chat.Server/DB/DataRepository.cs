using System.Collections.Generic;
using System.Linq;

using Chat.Server.Logging;
namespace Chat.DB
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
            try
            {
                return DB.Messeges.ToList();
            }
            catch
            {
                Log log = new Log();
                log.Error("ServerDataGetError(DataRepository)");
                return null;
            }


        }

        public void Save(Data data)
        {


            try
            {
                DB.Messeges.Add(data);
                DB.SaveChanges();
            }
            catch
            {
                Log log = new Log();
                log.Error("ServerDataSaveError:" + " " + data + " Nick: " + data.Nick + " Time " + data.Time + " Content: " + data.Content);
            }
        }
    }
}
