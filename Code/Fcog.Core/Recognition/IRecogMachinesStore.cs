using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Fcog.Core.Recognition
{
  public  interface IRecogMachinesStore
  {
      Task<RecogMachine> GetRecogMachineAsync(Guid recogMachineGuid);
      Task SaveRecogMachineAsync(RecogMachine recogMachine);
      Task<ReadOnlyCollection<RecogMachine>> GetRecogMachinesAsync();
  }
}
