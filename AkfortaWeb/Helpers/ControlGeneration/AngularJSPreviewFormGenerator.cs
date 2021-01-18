using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EECExchange.ControlGeneration.Controls;

namespace AkfortaWeb.Helpers.ControlGeneration
{
   public static class AngularJSPreviewFormGenerator
   {
      public static string GenerateHtml(ControlInfo[] controls, int columnsCount, string modelAccessPrefix, string scopePath, out string scopeEvalCode)
      {
         var scopeMembers = new Dictionary<string, string>();
         var html = generateHtml(controls, columnsCount, modelAccessPrefix, scopeMembers);

         scopeEvalCode = string.Join(Environment.NewLine,
            scopeMembers.Select(x => $"{scopePath}.{x.Key} = {x.Value};"));

         return html;
      }

      private static string generateHtml(ControlInfo[] controls, int columnsCount, string modelAccessPrefix, Dictionary<string, string> scopeMembers)
      {
         var sb = new StringBuilder();

         sb.Append("<table style=\"width: 100%;\"><tr>");

         var currentColNum = 0;
         foreach (var control in controls.OrderBy(x => x.OrderNum))
         {
            var newLine = false;

            if (control.ControlType == ControlType.SimpleControl && control.DataType != DataType.Complex)
            {
               if (currentColNum + control.ColumnSpan > columnsCount)
               {
                  sb.AppendLine("</tr><tr>");
                  currentColNum = 0;
               }
               currentColNum += control.ColumnSpan;
               sb.AppendLine($"<td colspan=\"{control.ColumnSpan}\">{control.Caption}:</td>");
               sb.Append(
                  $"<td colspan=\"{control.ColumnSpan}\"><input type=\"{getControlType(control.DataType)}\" {(control.DataType == DataType.DateTime ? "step=\"1\" " : "")}ng-model=\"{getModelFieldAccess(control, modelAccessPrefix)}\" readonly=\"readonly\"");
               if (control.DataType == DataType.Enum || control.DataType == DataType.String)
                  sb.Append($" title=\"{{{{{getModelFieldAccess(control, modelAccessPrefix)}}}}}\"");
               if (control.DataType == DataType.Boolean)
                  sb.Append(" onclick=\"return false;\"");
               sb.AppendLine("></input></td>");
            }

            if (control.ControlType == ControlType.SimpleControl && control.DataType == DataType.Complex)
            {
               var rowPrefix = currentColNum != 0 ? "</tr><tr>" : string.Empty;
               sb.AppendLine($"{rowPrefix}<td colspan=\"{columnsCount*2}\"><fieldset><legend>{control.Caption}</legend>");
               sb.AppendLine(generateHtml(control.Children, columnsCount,
                  getModelFieldAccess(control, modelAccessPrefix), scopeMembers));
               sb.AppendLine("</fieldset></td></tr><tr>");
               newLine = true;
            }
            
            if (control.ControlType == ControlType.GroupBox)
            {
               var rowPrefix = currentColNum != 0 ? "</tr><tr>" : string.Empty;
               sb.AppendLine($"{rowPrefix}<td colspan=\"{columnsCount*2}\"><fieldset><legend>{control.Caption}</legend>");
               sb.AppendLine(generateHtml(control.Children, columnsCount, modelAccessPrefix, scopeMembers));
               sb.AppendLine("</fieldset></td></tr><tr>");
               newLine = true;
            }

            if (control.ControlType == ControlType.Grid)
            {
               var rowPrefix = currentColNum != 0 ? "</tr><tr>" : string.Empty;
               sb.AppendLine($"{rowPrefix}<td colspan=\"{columnsCount * 2}\"><fieldset><legend>{control.Caption}</legend>");
               sb.AppendLine(generateGrid(control, columnsCount, modelAccessPrefix, scopeMembers));
               sb.AppendLine("</fieldset></td></tr><tr>");
               newLine = true;
            }

            if (newLine || control.BreakLine)
            {
               currentColNum = 0;
               sb.Append("</tr><tr>");
            }
         }

         sb.Append("</tr></table>");

         return sb.ToString();
      }

      private static string generateGrid(ControlInfo gridControl, int formColumnsCount, string modelAccessPrefix, Dictionary<string, string> scopeMembers)
      {
         var sb = new StringBuilder();

         var id = Guid.NewGuid().ToString("N");
         var popupId = $"popup{id}";
         var scopeVarId = $"currentItem{id}";
         var gridSettingsId = $"gridSettings{id}";
         var rowDoubleClickFuncId = $"rowDoubleClick{id}";

         sb.Append(
            $"<div ui-grid=\"{gridSettingsId}\" ui-grid-resize-columns ui-grid-move-columns ui-grid-selection ui-grid-auto-resize></div>");

         var gridSettings = new StringBuilder();
         gridSettings.Append(
            $"{{ data: '{modelAccessPrefix}.{gridControl.PropertyName}', noUnselect: true, multiSelect: false, enableRowHeaderSelection: false, columnDefs: [");

         foreach (var field in gridControl.Children.OrderBy(x => x.OrderNum))
         {
            var fieldName = getGridFieldPath(field, out var resolved);
            if (!resolved) continue;
            gridSettings.Append("{");
            if (!string.IsNullOrEmpty(fieldName))
               gridSettings.Append($"name: '{fieldName}', ");
            else
            {
               gridSettings.Append($"name: 'emptyName{id}', ");
               if (field.DataType != DataType.Boolean)
                  gridSettings.Append("cellTemplate: '<div>{{row.entity}}</div>', ");
            }

            gridSettings.Append($"displayName: '{field.Caption}', enableColumnMenu: false, minWidth: 200, ");
            if (field.DataType == DataType.DateTime)
               gridSettings.Append("cellFilter: 'date:\\'dd.MM.yyyy HH:mm:ss\\'', ");
            if (field.DataType == DataType.Boolean)
               gridSettings.Append(
                  $"cellTemplate: '<input type=\"checkbox\" ng-model=\"row.entity{(!string.IsNullOrEmpty(fieldName) ? "." + fieldName : "")}\" onclick=\"return false;\" style=\"horizontal-align: middle;\"></input>', ");
            gridSettings.Append("},");
         }
         gridSettings.Append("], ");
         var currentItemPath = $"{scopeVarId}";

         gridSettings.Append(
            $@"rowTemplate: '<div ng-dblclick=""grid.appScope.{rowDoubleClickFuncId}(row)"" ng-repeat=""(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name"" class=""ui-grid-cell"" ng-class=""{{ \'ui-grid-row-header-cell\': col.isRowHeader }}"" ui-grid-cell ></div>'");

         gridSettings.Append(@"};");
         scopeMembers.Add(gridSettingsId, gridSettings.ToString());
         scopeMembers.Add(rowDoubleClickFuncId, $"function(row){{row.grid.appScope.{currentItemPath} = row.entity; document.getElementById('{popupId}').style.display='block';}}");

         sb.Append($"<div id=\"{popupId}\" style=\"{PopupStyle}\"><div style=\"{PopupContentStyle}\">");
         sb.Append(
            $@"<button style=""border: none; color: #aaaaaa; float: right; font-size: 28px; font-weight: bold;"" onclick=""document.getElementById('{popupId}').style.display='none';"">&times;</button>");
         sb.Append($"<header><h1>{gridControl.TableItemCaption}</h1></header>");
         sb.Append(generateHtml(gridControl.Children, formColumnsCount, currentItemPath, scopeMembers));
         sb.Append(
            $@"<button onclick=""document.getElementById('{popupId}').style.display='none';"">Закрыть</button>");
         sb.Append("</div></div>");

         return sb.ToString();
      }

      private const string PopupStyle =
         "display: none; position: fixed; z-index: 1; padding-top: 10px; left: 0; top: 0; width: 100%; height: 100%; overflow: auto; background-color: rgba(0,0,0,0.3);";

      private const string PopupContentStyle =
         "background-color: #fefefe; margin: auto; padding: 10px; border: 1px solid #888; width: 90%;";

      private static string getGridFieldPath(ControlInfo control, out bool resolved)
      {
         var name = control.PropertyName;
         if (control.DataType == DataType.Complex && string.IsNullOrEmpty(control.ComplexTypeKeyProperty))
         {
            resolved = false;
            return name;
         }
         if (control.DataType == DataType.Complex && !string.IsNullOrEmpty(control.ComplexTypeKeyProperty))
         {
            var candidates = control.Children.Union(control.Children.Where(x => x.ControlType == ControlType.GroupBox)
               .SelectMany(x => x.Children ?? new ControlInfo[0])).ToArray();
            var item = candidates.FirstOrDefault(x => x.PropertyName == control.ComplexTypeKeyProperty);
            if (item != null)
               return $"{name}.{getGridFieldPath(item, out resolved)}";
         }

         resolved = true;
         return name;
      }

      private static string getControlType(DataType dataType)
      {
         switch (dataType)
         {
            case DataType.String:
               return "text";
            case DataType.Numeric:
               return "number";
            case DataType.Boolean:
               return "checkbox";
            case DataType.DateTime:
               return "datetime-local";
            case DataType.Enum:
               return "text";
            default:
               return "text";
         }
      }

      private static string getModelFieldAccess(ControlInfo control, string modelAccessPrefix)
      {
         if (string.IsNullOrEmpty(control.PropertyName))
            return modelAccessPrefix;
         return string.IsNullOrEmpty(modelAccessPrefix)
            ? control.PropertyName
            : $"{modelAccessPrefix}.{control.PropertyName}";
      }
   }
}