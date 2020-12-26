using DataRecorder.DataBases;
using DataRecorder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Installers
{
    public class DataRecorderAppInstaller : Zenject.Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<Repository>().AsSingle();
            this.Container.BindInterfacesAndSelfTo<GameStatus>().AsSingle();
        }
    }
}
