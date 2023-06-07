using UnityEngine;
using UnityEditor;
using UnityEditor.Localization;
using System.IO;
using System.Text;
using UnityEditor.Localization.Plugins.CSV;

public class ExportLocalizationTables
{


    [MenuItem("Localization/CSV/Export All CSV Files")]
    public static void ExportAllCsv()
    {
        // Get every String Table Collection
        var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

        var path = EditorUtility.SaveFolderPanel("Export String Table Collections - CSV", "", "");
        if (string.IsNullOrEmpty(path))
            return;

        foreach (var collection in stringTableCollections)
        {
            var file = Path.Combine(path, collection.TableCollectionName + ".csv");
            using (var stream = new StreamWriter(file, false, Encoding.UTF8))
            {
                Csv.Export(stream, collection);
            }
        }
    }


}
