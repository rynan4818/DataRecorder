using DataRecorder.DataBases;
using DataRecorder.Models;
using Zenject;

namespace DataRecorder.Installers
{
    public class DataRecorderAppInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<Repository>().AsSingle();
            this.Container.BindInterfacesAndSelfTo<GameStatus>().AsSingle();
        }
    }
}
