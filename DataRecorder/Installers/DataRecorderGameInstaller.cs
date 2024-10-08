﻿using DataRecorder.Models;
using Zenject;

namespace DataRecorder.Installers
{
    public class DataRecorderGameInstaller : Installer
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ScoreManager>().AsCached().NonLazy();
        }
    }
}
