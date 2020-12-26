using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace DataRecorder.Interfaces
{
    public interface IRepository : IInitializable, IDisposable
    {
    }
}
