using DataRecorder.Util;
using DataRecorder.Models;
using DataRecorder.Configuration;
using DataRecorder.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace DataRecorder.DataBases
{
    /// <summary>
    /// SQLiteDataBaseへの接続を提供するクラスです。
    /// 使う場合はZenjectのDIコンテナから引っ張ってきてください。
    /// </summary>
    public class Repository : IRepository, IInitializable, IDisposable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;");
            if (!File.Exists(PluginConfig.DataBaseFilePath)) {
                if (!Directory.Exists(Path.GetDirectoryName(PluginConfig.DataBaseFilePath))) {
					Directory.CreateDirectory(Path.GetDirectoryName(PluginConfig.DataBaseFilePath));
                }
				this.CreateTable();
            }
        }

		/// <summary>
		/// pause , resume イベント記録
		/// </summary>
		public void PaseEventAdd(string bs_event)
        {
			this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;");
			this._connection.Open();
			try {
				using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
					command.CommandText = "insert into MovieCutPause(time, event) values (@time, @event)";
					command.Parameters.Add(new SQLiteParameter("@time", Utility.GetCurrentTime()));
					command.Parameters.Add(new SQLiteParameter("@event", bs_event));
					command.ExecuteNonQuery();
				}
			}
			catch (Exception e) {
				Logger.Error(e);
			}
			finally {
				this._connection.Close();
			}
		}
		#endregion
		//ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
		#region // プライベートメソッド
		/// <summary>
		/// テーブルを作成します。
		/// </summary>
		private void CreateTable()
        {
			this._connection.Open();
			try {
				using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
					command.CommandText = @"
						CREATE TABLE IF NOT EXISTS MovieCutRecord(
							startTime INTEGER NOT NULL PRIMARY KEY,
							endTime INTEGER,
							menuTime INTEGER NOT NULL,
							cleared TEXT,
							endFlag INTEGER NOT NULL,
							pauseCount INTEGER NOT NULL,
							pluginVersion TEXT,
							gameVersion TEXT,
							scene TEXT,
							mode TEXT,
							songName TEXT,
							songSubName TEXT,
							songAuthorName TEXT,
							levelAuthorName TEXT,
							songHash TEXT,
							levelId TEXT,
							songBPM REAL,
							noteJumpSpeed REAL,
							songTimeOffset INTEGER,
							start TEXT,
							paused TEXT,
							length INTEGER,
							difficulty TEXT,
							notesCount INTEGER,
							bombsCount INTEGER,
							obstaclesCount INTEGER,
							maxScore INTEGER,
							maxRank TEXT,
							environmentName TEXT,
							scorePercentage REAL,
							score INTEGER,
							currentMaxScore INTEGER,
							rank TEXT,
							passedNotes INTEGER,
							hitNotes INTEGER,
							missedNotes INTEGER,
							lastNoteScore INTEGER,
							passedBombs INTEGER,
							hitBombs INTEGER,
							combo INTEGER,
							maxCombo INTEGER,
							multiplier REAL,
							obstacles TEXT,
							instaFail INTEGER,
							noFail INTEGER,
							batteryEnergy INTEGER,
							disappearingArrows INTEGER,
							noBombs INTEGER,
							songSpeed TEXT,
							songSpeedMultiplier REAL,
							noArrows INTEGER,
							ghostNotes INTEGER,
							failOnSaberClash INTEGER,
							strictAngles INTEGER,
							fastNotes INTEGER,
							staticLights INTEGER,
							leftHanded INTEGER,
							playerHeight REAL,
							reduceDebris INTEGER,
							noHUD INTEGER,
							advancedHUD INTEGER,
							autoRestart INTEGER
						);
					";
					command.ExecuteNonQuery();
					command.CommandText = @"
						CREATE TABLE IF NOT EXISTS MovieCutPause(
							time INTEGER NOT NULL PRIMARY KEY,
							event TEXT
						);
					";
					command.ExecuteNonQuery();
					command.CommandText = @"
						CREATE TABLE IF NOT EXISTS NoteScore(
							time INTEGER,
							cutTime INTEGER,
							startTime INTEGER,
							event TEXT,
							score INTEGER,
							currentMaxScore INTEGER,
							rank TEXT,
							passedNotes INTEGER,
							hitNotes INTEGER,
							missedNotes INTEGER,
							lastNoteScore INTEGER,
							passedBombs INTEGER,
							hitBombs INTEGER,
							combo INTEGER,
							maxCombo INTEGER,
							multiplier INTEGER,
							multiplierProgress REAL,
							batteryEnergy INTEGER,
							noteID INTEGER,
							noteType TEXT,
							noteCutDirection TEXT,
							noteLine INTEGER,
							noteLayer INTEGER,
							speedOK INTEGER,
							directionOK INTEGER,
							saberTypeOK INTEGER,
							wasCutTooSoon INTEGER,
							initialScore INTEGER,
							beforeScore INTEGER,
							afterScore INTEGER,
							cutDistanceScore INTEGER,
							finalScore INTEGER,
							cutMultiplier INTEGER,
							saberSpeed REAL,
							saberDirX REAL,
							saberDirY REAL,
							saberDirZ REAL,
							saberType TEXT,
							swingRating REAL,
							swingRatingFullyCut REAL,
							timeDeviation REAL,
							cutDirectionDeviation REAL,
							cutPointX REAL,
							cutPointY REAL,
							cutPointZ REAL,
							cutNormalX REAL,
							cutNormalY REAL,
							cutNormalZ REAL,
							cutDistanceToCenter REAL,
							timeToNextBasicNote REAL
						);
					";
					command.ExecuteNonQuery();
					DbColumnCheck(command, "MovieCutRecord", "levelId", "TEXT");
					DbColumnCheck(command, "NoteScore", "beforeScore", "INTEGER");
				}
			}
			catch (Exception e) {
				Logger.Error(e);
			}
			finally {
				this._connection.Close();
			}
		}

		private void DbColumnCheck(SQLiteCommand db_cmd, string table, string column, string type)
		{
			db_cmd.CommandText = $"PRAGMA table_info('{table}');";
			bool column_check = true;
			using (SQLiteDataReader db_reader = db_cmd.ExecuteReader()) {
				while (db_reader.Read()) {
					if (column == (string)db_reader["name"]) {
						column_check = false;
						break;
					}
				}
			}
			if (column_check) {
				db_cmd.CommandText = $"ALTER TABLE {table} ADD COLUMN {column} {type};";
				db_cmd.ExecuteNonQuery();
			}
		}
		#endregion
		//ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
		#region // メンバ変数
		private SQLiteConnection _connection;

		[Inject]
		private GameStatus _gameStatus;

		#endregion
		//ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
		#region // 構築・破棄
		public Repository()
        {
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    this._connection?.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Repository()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
