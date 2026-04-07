using UnityEngine;
using UnityEditor;
using System.IO;

public class ExtractFBXMaterialsAndTextures
{
    // Dossier parent déjà créé dans ton projet
    private const string ROOT_FOLDER = "Assets/materials Julien";

    [MenuItem("Tools/Julien/Extract Textures + Materials From Selected FBX")]
    static void ExtractSelectedFBX()
    {
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "Aucune sélection",
                "Sélectionne au moins un fichier FBX dans le Project window.",
                "OK"
            );
            return;
        }

        if (!AssetDatabase.IsValidFolder(ROOT_FOLDER))
        {
            EditorUtility.DisplayDialog(
                "Dossier introuvable",
                $"Le dossier \"{ROOT_FOLDER}\" n'existe pas.\nCrée-le d'abord dans Assets.",
                "OK"
            );
            return;
        }

        int processedCount = 0;

        foreach (Object obj in selectedObjects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);

            if (string.IsNullOrEmpty(assetPath) || !assetPath.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogWarning($"Ignoré : {assetPath} (pas un FBX)");
                continue;
            }

            ModelImporter importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (importer == null)
            {
                Debug.LogWarning($"Impossible de récupérer le ModelImporter pour : {assetPath}");
                continue;
            }

            string fbxName = Path.GetFileNameWithoutExtension(assetPath);
            string objectFolder = $"{ROOT_FOLDER}/{fbxName}";

            CreateFolderIfNeeded(ROOT_FOLDER, fbxName);

            // 1) Extraire les textures embedded dans le sous-dossier de l'objet
            try
            {
                importer.ExtractTextures(objectFolder);
                AssetDatabase.Refresh();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Erreur pendant l'extraction des textures pour {fbxName} : {e.Message}");
            }

            // 2) Extraire les matériaux embedded en .mat dans le même dossier
            Object[] embeddedAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (Object embedded in embeddedAssets)
            {
                if (embedded is Material)
                {
                    string safeMatName = MakeSafeFileName(embedded.name);
                    string matPath = $"{objectFolder}/{safeMatName}.mat";

                    // Si le matériau n'existe pas déjà, on l'extrait
                    if (AssetDatabase.LoadAssetAtPath<Material>(matPath) == null)
                    {
                        string error = AssetDatabase.ExtractAsset(embedded, matPath);

                        if (!string.IsNullOrEmpty(error))
                        {
                            Debug.LogWarning($"Impossible d'extraire le matériau {embedded.name} de {fbxName} : {error}");
                        }
                    }
                }
            }

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();

            // 3) Demander à Unity de remapper le FBX vers les matériaux externes trouvés
            importer = AssetImporter.GetAtPath(assetPath) as ModelImporter;
            if (importer != null)
            {
                importer.SearchAndRemapMaterials(
                    ModelImporterMaterialName.BasedOnMaterialName,
                    ModelImporterMaterialSearch.Everywhere
                );

                importer.SaveAndReimport();
            }

            processedCount++;
            Debug.Log($"Extraction terminée pour : {fbxName} -> {objectFolder}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog(
            "Terminé",
            $"Extraction finie.\nFBX traités : {processedCount}",
            "OK"
        );
    }

    private static void CreateFolderIfNeeded(string parentFolder, string childFolderName)
    {
        string folderPath = $"{parentFolder}/{childFolderName}";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder(parentFolder, childFolderName);
        }
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