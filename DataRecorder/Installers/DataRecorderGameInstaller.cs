using DataRecorder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiraUtil;

namespace DataRecorder.Installers
{
    public class DataRecorderGameInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ScoreManager>().AsCached();
        }
    }
}
