using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices.Filters;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_Get_lenght_polyline_03_05_2024
{
    public class Class1
    {
        [CommandMethod("U84PolyLenght")]
        public void SelectAllPolylineByLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

           StringBuilder Cable = new StringBuilder();
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                try
                {
                    PromptSelectionOptions opts = new PromptSelectionOptions();
                    opts.MessageForAdding = "Выберите кабели: ";
                    PromptSelectionResult res = ed.GetSelection(opts);
                    // Do nothing if selection is unsuccessful
                    if (res.Status != PromptStatus.OK)
                        return;
                    SelectionSet selSet = res.Value;
                    // добавляем в массив выбранные обьекты
                    ObjectId[] idArray = selSet.GetObjectIds();
                    foreach (ObjectId id in idArray)
                    {

                        DBObject obj = tr.GetObject(id, OpenMode.ForRead);
                        // выбирам из коллекции атрибутов по ссылкам на блоки
                        Polyline lwp = obj as Polyline; // Get the selected polyline during runtime
                       Cable.Append( lwp.Layer.ToString() + ";" );
                        Cable.Append(lwp.Length.ToString() + ";" );
                        Cable.Append("\n");
                    }
                    SaveCsv saveCsv = new SaveCsv();
                    saveCsv.saveCsv(Cable.ToString());
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                    tr.Abort();
                }
            }
            tr.Commit();
        }
    }
}
