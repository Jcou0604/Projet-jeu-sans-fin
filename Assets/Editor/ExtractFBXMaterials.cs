using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ExtractLikeManual
{
    private const string RootFolder = "Assets/materials Julien";

    [MenuItem("Tools/Julien/Extract Textures Then Materials")]
    public static void ExtractSelected()
    {
        Object[] selected = Selection.objects;

        if (selected == null || selected.Length == 0)
        {
            EditorUtility.DisplayDialog("Erreur", "Sélectionne au moins un FBX dans le Project window.", "OK");
            return;
        }

        if (!AssetDatabase.IsValidFolder(RootFolder))
        {
            EditorUtility.DisplayDialog("Erreur", $"Le dossier \"{RootFolder}\" n'existe pas.", "OK");
            return;
        }

        int processed = 0;

        foreach (Object obj in selected)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(assetPath) || !assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning($"Ignoré : {assetPath}");
                continue;
            }

            string fbxName = Path.GetFileNameWithoutExtension(assetPath);
            string targetFolder = $"{RootFolder}/{fbxName}";

            try
            {
                RecreateFolder(targetFolder);

                ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (importer == null)
                {
                    Debug.LogWarning($"Pas de ModelImporter pour : {assetPath}");
                    continue;
                }

                // 1) Extraire les textures d'abord
                bool texturesExtracted = ExtractTextures(importer, assetPath, targetFolder, fbxName);

                // 2) Extraire les matériaux embedded
                int materialsExtracted = ExtractEmbeddedMaterials(assetPath, targetFolder);

                // 3) Refresh / import pour que tout soit bien visible
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);

                // 4) Remap des matériaux
                importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
                if (importer != null)
                {
                    importer.SearchAndRemapMaterials(
                        ModelImporterMaterialName.BasedOnMaterialName,
                        ModelImporterMaterialSearch.Everywhere
                    );

                    importer.SaveAndReimport();
                }

                // 5) Petit log utile
                int textureCount = CountAssetsOfType<Texture2D>(targetFolder);
                int materialCount = CountAssetsOfType<Material>(targetFolder);

                Debug.Log(
                    $"[ExtractLikeManual] OK : {fbxName}\n" +
                    $"- Textures extraites : {(texturesExtracted ? "oui" : "non / aucune")}\n" +
                    $"- Nombre de textures dans le dossier : {textureCount}\n" +
                    $"- Matériaux extraits : {materialsExtracted}\n" +
                    $"- Nombre de matériaux dans le dossier : {materialCount}\n" +
                    $"- Dossier : {targetFolder}"
                );

                processed++;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[ExtractLikeManual] Erreur pour {fbxName} : {e}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Terminé", $"FBX traités : {processed}", "OK");
    }

    private static bool ExtractTextures(ModelImporter importer, string assetPath, string targetFolder, string fbxName)
    {
        try
        {
            importer.ExtractTextures(targetFolder);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);

            int textureCount = CountAssetsOfType<Texture2D>(targetFolder);

            if (textureCount == 0)
            {
                Debug.LogWarning($"[ExtractLikeManual] Aucune texture extraite pour {fbxName}");
                return false;
            }

            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[ExtractLikeManual] ExtractTextures a échoué pour {fbxName} : {e.Message}");
            return false;
        }
    }

    private static int ExtractEmbeddedMaterials(string assetPath, string targetFolder)
    {
        int extractedCount = 0;

        Object[] embeddedAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        foreach (Object asset in embeddedAssets)
        {
            if (!(asset is Material material))
                continue;

            string safeMatName = MakeSafeFileName(material.name);
            string matPath = $"{targetFolder}/{safeMatName}.mat";

            // Si déjà là, on le supprime pour repartir proprement
            if (AssetDatabase.LoadAssetAtPath<Material>(matPath) != null)
            {
                AssetDatabase.DeleteAsset(matPath);
            }

            string error = AssetDatabase.ExtractAsset(asset, matPath);

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogWarning($"[ExtractLikeManual] Erreur extraction matériau \"{material.name}\" : {error}");
                continue;
            }

            AssetDatabase.ImportAsset(matPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
            extractedCount++;
        }

        return extractedCount;
    }

    private static int CountAssetsOfType<T>(string folder) where T : Object
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder });
        return guids.Length;
    }

    private static void RecreateFolder(string folderPath)
    {
        if (AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.DeleteAsset(folderPath);
            AssetDatabase.Refresh();
        }

        string[] parts = folderPath.Split('/');
        string current = parts[0]; // "Assets"

        for (int i = 1; i < parts.Length; i++)
        {
            string next = $"{current}/{parts[i]}";
            if (!AssetDatabase.IsValidFolder(next))
            {
                AssetDatabase.CreateFolder(current, parts[i]);
            }
            current = next;
        }

        AssetDatabase.Refresh();
    }

    private static string MakeSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}