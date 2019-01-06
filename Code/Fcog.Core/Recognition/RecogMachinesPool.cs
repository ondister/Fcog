using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fcog.Core.Recognition
{
    public class RecogMachinesPool
    {
                
        #region thread safe Singlton

        private static RecogMachinesPool instance;
        private static readonly object syncRoot = new object();

        private RecogMachinesPool()
        {
            RecogMachines=new ReadOnlyCollection<RecogMachine>(new List<RecogMachine>());
        }

        public static RecogMachinesPool Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                    {
                        if (instance == null) { instance = new RecogMachinesPool(); }
                    }
                return instance;
            }
        }



        #endregion

        public ReadOnlyCollection<RecogMachine> RecogMachines { get; private set; }

        private IRecogMachinesStore store;

        public async Task InitializeStoreAsync(IRecogMachinesStore recogMachinesStore)
        {
            store = recogMachinesStore;
            RecogMachines = await store.GetRecogMachinesAsync();
            if (!RecogMachines.Any()) { throw new Exception("RecogMachine store havent any machines"); }
        }

        public RecogMachine GetRecogMachine(Guid guid)
        {
            if (store == null) { throw new Exception("RecogMachine store not Initialized. Initialize it first"); }
            
            var result = RecogMachines.FirstOrDefault(m => m.Id == guid);

            if (result == null) { throw new Exception("RecogMachine not found in the store"); }

            return result;
        }
    }
}
