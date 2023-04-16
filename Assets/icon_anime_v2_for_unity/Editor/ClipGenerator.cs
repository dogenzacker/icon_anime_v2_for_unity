using System.Collections.Generic;
using System.IO;
using System.Linq;
using icon_anime_v2;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class ClipGenerator : EditorWindow
{
    [MenuItem("icon_anime_v2/icon_anime_v2をUnityで使いたいマン")]
    static void Init()
    {
        ClipGenerator window = (ClipGenerator)EditorWindow.GetWindow(typeof(ClipGenerator));
        window.titleContent.text = "icon_anime_v2をUnityで使いたいマン";
        window.Show();
    }

    private Dictionary<string, bool> m_CheckSheet = new();
    private const string SourceDirectory = "Assets/icon_anime_v2/";
    private const string DestinationDirectory = "Assets/icon_anime_v2/clip";
    private const string BOOTH_URL = "https://booth.pm/ja/items/3122939";

    void OnGUI()
    {
        GUILayout.Label("⚠ アニメーションファイルの入手先 ⚠");
        GUIStyle linkStyle = new GUIStyle(GUI.skin.label);
        linkStyle.normal.textColor = Color.blue;
        linkStyle.stretchWidth = false;
        linkStyle.wordWrap = false;
        linkStyle.richText = true;
        if (GUILayout.Button($"<color=orange><b>{BOOTH_URL}</b></color>", linkStyle))
        {
            Application.OpenURL(BOOTH_URL);
        }
        GUILayout.Space(20);

        GUILayout.Label("⚠ 使い方 ⚠", EditorStyles.boldLabel);
        GUILayout.Label("・PackageManagerから2D Spriteをインポートしてください", EditorStyles.boldLabel);
        GUILayout.Label("・書き出したClipファイルをSpritePlayerやImagePlayerに設定してPlayメソッドを呼ぶことで再生することができます", EditorStyles.boldLabel);
        GUILayout.Label("・Addressablesで使用する事でファイルサイズを抑えることができるはずです", EditorStyles.boldLabel);
        GUILayout.Space(20);


        GUILayout.Label("⚠ 注意 ⚠", EditorStyles.boldLabel);
        GUILayout.Label("・実行前に編集中のシーンなどは保存しておいてください", EditorStyles.boldLabel);
        GUILayout.Label("・結構時間がかかります、必要なアニメーションだけ置きましょう", EditorStyles.boldLabel);
        GUILayout.Label("・変換中はメモリをたくさん使います", EditorStyles.boldLabel);
        GUILayout.Label("・書き出す時に元のテクスチャインポートの設定を変更します", EditorStyles.boldLabel);
        GUILayout.Label("・ファイル名がUTF-8-MacなのでUTF-8に変換します", EditorStyles.boldLabel);
        GUILayout.Label("・シーン上で使っている場合には使用するか考えてください", EditorStyles.boldLabel);
        GUILayout.Label("・textureType = TextureImporterType.Sprite", EditorStyles.boldLabel);
        GUILayout.Label("・alphaIsTransparency = true", EditorStyles.boldLabel);
        GUILayout.Label("・書き出しに使った画像の名前変更や削除をすると動かなくなります(移動はOK)", EditorStyles.boldLabel);
        GUILayout.Space(20);

        if (Directory.Exists(DestinationDirectory) == false)
        {
            Directory.CreateDirectory("Assets/icon_anime_v2/clip");
        }

        // var selectedDirectories = Selection
        //     .GetFiltered<DefaultAsset>(SelectionMode.TopLevel)
        //     .Select(x => AssetDatabase.GetAssetPath(x))
        //     .Where(x => AssetDatabase.IsValidFolder(x))
        //     .ToArray();

        // var selectedDirectories = Directory.GetFiles(SourceDirectory).Where(_ =>
        // {
        //     Debug.Log($"SourceDirectory:{_}");
        //     return Directory.Exists(_);
        // });

        var selectedDirectories = new DirectoryInfo(SourceDirectory).GetDirectories().Select(_ => SourceDirectory + _.Name);


        // 書き出し先設定
        // 書き出し先
        GUILayout.Label("⚠ 読み込み先 ⚠", EditorStyles.boldLabel);
        GUILayout.Label(SourceDirectory, EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("⚠ 書き出し先 ⚠", EditorStyles.boldLabel);
        GUILayout.Label(DestinationDirectory, EditorStyles.boldLabel);
        // if (GUILayout.Button("書き出し先変更"))
        // {
        //     var result = EditorUtility.OpenFolderPanel("書き出し先を設定", m_SelectedOutputDirectory, "");
        //     if (string.IsNullOrEmpty(result) == false)
        //     {
        //         m_SelectedOutputDirectory = result;
        //     }
        // }
        GUILayout.Space(20);


        var count = 0;
        if (GUILayout.Button("書き出し"))
        {
            foreach (var path in selectedDirectories)
            {
                if (m_CheckSheet[path] == false) continue;
                CreateSpriteAtlasFromDirectory(path, DestinationDirectory);
                m_CheckSheet[path] = false;
                count++;
            }
        }

        GUILayout.Label("⚠ チェックをすると書き出し対象 ⚠", EditorStyles.boldLabel);
        foreach (var path in selectedDirectories)
        {
            GUILayout.BeginHorizontal();

            if (m_CheckSheet.ContainsKey(path) == false)
            {
                m_CheckSheet[path] = true;
            }
            m_CheckSheet[path] = GUILayout.Toggle(m_CheckSheet[path], path);
            GUILayout.EndHorizontal();
        }

    }


    const string ORIGIN_NO = "_00000";
    const string PREFIX_REND = "_rend";
    const string PREFIX_LOOP = "_loop";
    const string PREFIX_PLAYONCE = "_playonce";
    const string PREFIX_15FPS = "15fps";

    // [MenuItem("Build/icon_anime_v2", false, 200)]
    private void CreateSpriteAtlasFromDirectory(string srcDirectoryPath, string dstDirectoryPath)
    {
        string prefix = "";
        string fileName = "";
        bool isLooping = false;
        float fps = 30;
        string[] pngPathes = Directory.GetFiles(srcDirectoryPath, "*.png");
        if (pngPathes.Length == 0) return;
        // 
        var sprites = new List<Sprite>();

        foreach (var pngPath in pngPathes)
        {

            var file_path = pngPath;
            if (file_path.IsNormalized() == false)
            {
                file_path = pngPath.Normalize();
                // UTF-8-Mac対策 
                AssetDatabase.MoveAsset(pngPath, file_path);
            }

            // ファイル名を作る
            if (file_path.LastIndexOf($"{ORIGIN_NO}.png") != -1)
            {
                fileName = Path.GetFileNameWithoutExtension(file_path).Replace(ORIGIN_NO, "");


                if (file_path.IndexOf(PREFIX_15FPS) != -1)
                {
                    prefix += PREFIX_15FPS;
                    fileName = fileName.Replace(PREFIX_15FPS, "");
                    fps = 15;
                }

                if (file_path.IndexOf(PREFIX_PLAYONCE) != -1)
                {
                    fileName = fileName.Replace(PREFIX_PLAYONCE, "");
                    isLooping = false;
                    prefix += PREFIX_PLAYONCE;
                }

                if (file_path.IndexOf(PREFIX_LOOP) != -1)
                {
                    fileName = fileName.Replace(PREFIX_LOOP, "");
                    isLooping = true;
                    prefix += PREFIX_LOOP;
                }

                if (file_path.IndexOf(PREFIX_REND) != -1)
                {
                    fileName = fileName.Replace(PREFIX_REND, "");
                    prefix += PREFIX_REND;
                }
            }

            // Textureのインポートの設定をSpriteに変更する 
            TextureImporterSettings settings = new TextureImporterSettings();
            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(file_path);
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.alphaIsTransparency = true;
            textureImporter.ReadTextureSettings(settings);
            textureImporter.SetTextureSettings(settings);
            AssetDatabase.ImportAsset(file_path);
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(file_path);
            sprites.Add(sprite);
        }

        SpriteAtlas spriteAtlas = new SpriteAtlas();
        spriteAtlas.Add(sprites.ToArray());

        /* 
            spriteAtlas.SetIncludeInBuild(bool);
            true:本体に組み込む場合(ファイルサイズが大きくなる)
            false:Addressablesなどで使用する場合
        */
        spriteAtlas.SetIncludeInBuild(true);

        var packingSettings = spriteAtlas.GetPackingSettings();
        packingSettings.enableTightPacking = false;
        spriteAtlas.SetPackingSettings(packingSettings);
        Debug.Log("directoryPath:" + srcDirectoryPath);
        // const string outDirectory = "Assets/icon_anime_v2_assets/";
        var atlasPath = $"{dstDirectoryPath}/{fileName}.spriteatlas";
        AssetDatabase.CreateAsset(spriteAtlas, atlasPath);
        var prefabPath = $"{dstDirectoryPath}/{fileName}.asset";
        var icon_anime_v2_prefab = Clip.Generate(spriteAtlas, prefix, fps, isLooping);

        AssetDatabase.CreateAsset(icon_anime_v2_prefab, prefabPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        foreach (var sprite in sprites)
        {
            Resources.UnloadAsset(sprite);
        }
        Resources.UnloadUnusedAssets();
    }
}
