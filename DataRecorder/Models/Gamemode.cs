using System;
using Zenject;

namespace DataRecorder.Models
{
    public class Gamemode : IInitializable, IDisposable
    {
        private readonly GameStatus _gameStatus;
        private readonly MainMenuViewController _mainMenuViewController;
        private bool _disposedValue;
        public Gamemode(GameStatus gameStatus, MainMenuViewController mainMenuViewController)
        {
            this._gameStatus = gameStatus;
            this._mainMenuViewController = mainMenuViewController;
        }
        public void Initialize()
        {
            this._mainMenuViewController.didFinishEvent += this.OnMainMenuViewController_didFinishEvent;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._mainMenuViewController.didFinishEvent -= this.OnMainMenuViewController_didFinishEvent;
                }
                this._disposedValue = true;
            }
        }
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        private void OnMainMenuViewController_didFinishEvent(MainMenuViewController arg1, MainMenuViewController.MenuButton arg2)
        {
            switch (arg2)
            {
                case MainMenuViewController.MenuButton.SoloFreePlay:
                    this._gameStatus.partyMode = false;
                    this._gameStatus.multiplayer = false;
                    this._gameStatus.campaign = false;
                    break;
                case MainMenuViewController.MenuButton.Party:
                    this._gameStatus.partyMode = true;
                    this._gameStatus.multiplayer = false;
                    this._gameStatus.campaign = false;
                    break;
                case MainMenuViewController.MenuButton.Multiplayer:
                    this._gameStatus.partyMode = false;
                    this._gameStatus.multiplayer = true;
                    this._gameStatus.campaign = false;
                    break;
                case MainMenuViewController.MenuButton.SoloCampaign:
                    this._gameStatus.partyMode = false;
                    this._gameStatus.multiplayer = false;
                    this._gameStatus.campaign = true;
                    break;
                case MainMenuViewController.MenuButton.BeatmapEditor:
                case MainMenuViewController.MenuButton.FloorAdjust:
                case MainMenuViewController.MenuButton.Quit:
                case MainMenuViewController.MenuButton.Options:
                case MainMenuViewController.MenuButton.HowToPlay:
                default:
                    this._gameStatus.partyMode = false;
                    this._gameStatus.multiplayer = false;
                    this._gameStatus.campaign = false;
                    break;
            }
        }
    }
}
