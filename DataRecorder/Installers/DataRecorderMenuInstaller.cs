using DataRecorder.Models;
using Zenject;

namespace DataRecorder.Installers
{
    public class DataRecorderMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<Gamemode>().AsCached().NonLazy();
        }
    }
}
