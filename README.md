# DataRecorder
このBeatSaberプラグインは、プレイ動画カットツール（[BS Movie Cut](https://github.com/rynan4818/bs-movie-cut)）用のプレイ情報記録用modです。

BeatSaber1.13.0までは[Beat Saber HTTP Status +Database](https://github.com/rynan4818/beatsaber-http-status-db)としてリリースしていた物を、BeatSaber1.13.2より記録機能を単独化しました。

オーバーレイ用には別途下記のプラグインを使用して下さい。
- [Beat Saber HTTP Status](https://github.com/opl-/beatsaber-http-status)
- [Beat Saber HTTP Status](https://github.com/denpadokei/beatsaber-http-status)※デンバ時計さんの、パフォーマンス改善版
- [DataPuller](https://github.com/kOFReadie/BSDataPuller)

※HTTP Statusと、DataPullerは互換性は無いのでそれぞれ専用のオーバーレイが必要です
- HTTP Status用オーバーレイ [Beat Saber Overlay 改良版](https://github.com/rynan4818/beat-saber-overlay)

# インストール方法

1. [リリースページ](https://github.com/rynan4818/DataRecorder/releases)から最新のリリースをダウンロードします。
2. zipファイルをBeat Saberフォルダに解凍します。(具体的には以下の様になります)
    1. `Libs`フォルダに`SQLite.Interop.dll`と`System.Data.SQLite.dll`ファイルをコピーします。
        - 従来のHTTP Status+DBで`Beat Saber_Data\Managed`にコピーしていた同名のDLLファイルが残っている場合は削除して下さい。
    2. `Plugins`フォルダに`DataRecorder.dll`ファイルをコピーします。
3. このmodは以下のプラグインに依存するため、[ModAssistant](https://github.com/Assistant/ModAssistant)でインストールして下さい。
    - BSIPA 4.1.4以上
    - BS Utils 1.7.0以上
    - SiraUtil 2.4.0以上
4. **従来のHTTP Status+DBを使用していた方は、データベースの場所が変更になっていますので下記手順で移動して下さい。**
    1. .一度BeatSaberを起動して終了して下さい。
    2. `UserData`フォルダに`DataRecorder`フォルダが作成され、その中に`beatsaber.db`が作成されます。
    3. 今後はこのフォルダで記録するため、従来の`UserData`フォルダ直下にあった`beatsaber.db`を移動して上書きして下さい。
    4. BS Movie Cutのメニューの`オプション`の`設定`からbeatsaber.dbファイルの場所を変更して下さい。
# 使用方法
modのインストールが完了した状態でBeatSaberをプレイすると`UserData\DataRecorder\beatsaber.db`ファイルが作成され、プレイ情報を記録します。
プレイ動画カットツール（[BS Movie Cut](https://github.com/rynan4818/bs-movie-cut)）で、記録したデータベースを設定して、録画したプレイ動画のカット編集などが自動で可能です。
# 設定
`UserData`フォルダに`DataRecorder.json`ファイルが作成されます。`DBFilePath`の項目にデーターベースファイルの保存パスが設定されます。標準とは異なる場所にデーターベースを保存する場合は手動で変更して下さい。将来的にはBS Movie Cutの設定画面で変更可能にする予定です。
# 謝辞
本modツールの作成にあたり[デンパ時計さん](https://github.com/denpadokei)にmod作成の土台に当たる部分を作って頂きました。
新規のmod作成は始めてでしたので、大変助かり勉強になりました。心から感謝いたします。

また、ソースコードの多くの部分を[HTTP Status](https://github.com/opl-/beatsaber-http-status)から流用しています。素晴らしいツールを作って頂いたopl氏に感謝します。
