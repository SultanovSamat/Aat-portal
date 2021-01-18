using System;
using System.Collections.Generic;
using System.Linq;
using EECExchange.ControlGeneration.Attributes;
using EECExchange.ControlGeneration.Controls;

namespace AkfortaWeb.Helpers.ControlGeneration
{
   public static class ControlGenerationProvider
   {
      public static ControlInfo[] GetAllControls(Type viewModelType)
      {
         return mergeGroups(
            GetSimpleControls(viewModelType).Union(GetTables(viewModelType)).ToArray()
         );
      }

      public static ControlInfo[] GetSimpleControls(Type viewModelType)
      {
         var controls = new List<ControlInfo>();

         var fieldProps = viewModelType.GetProperties().Select(x => new
         {
            Prop = x,
            Attr = x.GetCustomAttributes(typeof(DisplayFieldAttribute), true).Cast<DisplayFieldAttribute>()
               .FirstOrDefault()
         }).Where(x => x.Attr != null).ToArray();

         var propertiesGroup = fieldProps.GroupBy(x => x.Attr.GroupName ?? string.Empty)
            .Select(x => new {Order = x.Min(y => y.Attr.GroupDisplayOrder), Group = x});

         foreach (var propGroup in propertiesGroup)
         {
            var controlItems = string.IsNullOrEmpty(propGroup.Group.Key) ? controls : new List<ControlInfo>();

            foreach (var prop in propGroup.Group)
            {
               var propType = GetDataType(prop.Prop.PropertyType);

               var item = new ControlInfo
               {
                  Caption = prop.Attr.DisplayName,
                  DataType = propType,
                  BreakLine = prop.Attr.BreakLine,
                  ControlType = ControlType.SimpleControl,
                  OrderNum = prop.Attr.DisplayOrder,
                  ColumnSpan = prop.Attr.ColumnSpan,
                  PropertyName = prop.Prop.Name
               };

               if (propType == DataType.Complex)
               {
                  //TODO add support of ItemsSourcePropertyBinding
                  item.Children = GetAllControls(prop.Attr.ComplexTypeViewModel);
                  item.ComplexTypeKeyProperty = prop.Attr.ComplexTypeKeyProperty;
               }

               controlItems.Add(item);
            }

            if (!string.IsNullOrEmpty(propGroup.Group.Key))
               controls.Add(createGroup(propGroup.Group.Key, propGroup.Order, controlItems.ToArray()));
         }

         return controls.ToArray();
      }

      public static ControlInfo[] GetTables(Type viewModelType)
      {
         var controls = new List<ControlInfo>();

         var tableProps = viewModelType.GetProperties().Select(x => new
         {
            Prop = x,
            Attr = x.GetCustomAttributes(typeof(DisplayTableAttribute), true).Cast<DisplayTableAttribute>()
               .FirstOrDefault()
         }).Where(x => x.Attr != null).ToArray();

         var tablesGroup = tableProps.GroupBy(x => x.Attr.GroupName ?? string.Empty)
            .Select(x => new { Order = x.Min(y => y.Attr.GroupDisplayOrder), Group = x });

         foreach (var tableGroup in tablesGroup)
         {
            var controlItems = string.IsNullOrEmpty(tableGroup.Group.Key) ? controls : new List<ControlInfo>();

            foreach (var prop in tableGroup.Group)
            {
               var item = new ControlInfo
               {
                  Caption = prop.Attr.TableName,
                  BreakLine = true,
                  ControlType = ControlType.Grid,
                  OrderNum = prop.Attr.DisplayOrder,
                  PropertyName = prop.Prop.Name,
                  TableItemCaption = prop.Attr.ElementCaption,
                  Children = prop.Attr.ViewModelType != null
                     ? GetAllControls(prop.Attr.ViewModelType)
                     : new[]
                     {
                        new ControlInfo
                        {
                           Caption = prop.Attr.TableName, ControlType = ControlType.SimpleControl, OrderNum = 1,
                           DataType = GetDataType(prop.Attr.ModelType)
                        }
                     }
               };

               controlItems.Add(item);
            }

            if (!string.IsNullOrEmpty(tableGroup.Group.Key))
               controls.Add(createGroup(tableGroup.Group.Key, tableGroup.Order, controlItems.ToArray()));
         }

         return controls.ToArray();
      }

      private static ControlInfo createGroup(string caption, int order, ControlInfo[] childItems)
      {
         return new ControlInfo
         {
            Caption = caption,
            OrderNum = order,
            Children = childItems,
            ControlType = ControlType.GroupBox
         };
      }

      private static ControlInfo[] mergeGroups(ControlInfo[] controlInfos)
      {
         var result = new List<ControlInfo>(controlInfos.Where(x => x.ControlType != ControlType.GroupBox));
         var groupGroups = controlInfos.Where(x => x.ControlType == ControlType.GroupBox).GroupBy(x => x.Caption);
         foreach (var gr in groupGroups)
         {
            if (gr.Count() > 1)
            {
               result.Add(createGroup(gr.Key, gr.Min(x => x.OrderNum),
                  gr.SelectMany(x => x.Children).ToArray()));
            }
            else
               result.AddRange(gr);
         }

         return result.ToArray();
      }

      internal static DataType GetDataType(Type type)
      {
         if (type == typeof(string))
            return DataType.String;
         if (type == typeof(DateTime) || type == typeof(DateTime?))
            return DataType.DateTime;
         if (type == typeof(bool) || type == typeof(bool?))
            return DataType.Boolean;
         if (type == typeof(byte) || type == typeof(float) ||
             type == typeof(int) || type == typeof(uint) ||
             type == typeof(short) || type == typeof(ushort) ||
             type == typeof(long) || type == typeof(ulong) ||
             type == typeof(decimal) || type == typeof(double) ||
             type == typeof(byte?) || type == typeof(float?) ||
             type == typeof(int?) || type == typeof(uint?) ||
             type == typeof(short?) || type == typeof(ushort?) ||
             type == typeof(long?) || type == typeof(ulong?) ||
             type == typeof(decimal?) || type == typeof(double?))
            return DataType.Numeric;
         if (type.IsEnum)
            return DataType.Enum;

         return DataType.Complex;
      }
   }
}