using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using AkfortaWeb.Helpers;

namespace AkfortaWeb.Models
{
   [Serializable]
   [XmlRoot("Regions"), XmlType("Region")]
   public class Region
   {
      public Guid UID { get; set; }

      public string Code { get; set; }

      public string Name { get; set; }

      public Region()
      {
      }

      public Region(DataRow row)
      {
         Name = row[0].ToString();
         Code = row[1].ToString();
      }

      public static void SetXmlData(string file)
      {
         if (XmlData == null || SerializeHelper.ShouldUpdate(file))
         {
            XmlData = SerializeHelper.GetXmlData<Region>(file);
         }
      }

      public static List<Region> GetXmlData()
      {
         SetXmlData("regions");
         return XmlData;
      }

      private static List<Region> XmlData { get; set; }

      private static List<RH> RHList { get; set; }

      public static object[] GetHierarchicalData()
      {
         SetXmlData("regions");
         RHList = new List<RH>();
         var MainRegion = GetMainRegion();
         var rh = new RH();
         rh.name = MainRegion.Name;
         rh.code = MainRegion.Code;
         rh.hasChildren = HasChildren(MainRegion.Code);
         RHList.Add(rh);
         return RHList.ToArray();
      }

      static bool HasChildren(string code)
      {
         string reg = UtialRegular(code);
         System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(reg);
         return XmlData.Any(x => regex.IsMatch(code) && x.Code != code);
      }

      private static Region GetMainRegion()
      {
         var reg = UtialRegular(string.Empty);
         System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(reg);
         var region = XmlData.FirstOrDefault(x => regex.IsMatch(x.Code));

         return region;
      }

      internal static object[] GetSubRegions(string code)
      {
         List<RH> subRegions = new List<RH>();
         string reg = UtialRegular(code);
         System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(reg);
         subRegions = XmlData.Where(x => regex.IsMatch(x.Code) && x.Code != code)
            .Select(x => new RH {code = x.Code, name = x.Name, hasChildren = HasChildren(x.Code)}).ToList();
         return subRegions.ToArray();
      }

      private static string UtialRegular(string cod)
      {
         if (cod == string.Empty)
            return @"(\d{3})00000000000";

         for (int i = cod.Count() - 1; i >= 0; i--)
         {
            if (cod[i] == '0')
               cod = cod.Substring(0, i);
            else
               break;
         }

         string RegGroup = string.Empty;
         switch (cod.Count())
         {
            case 3:
            {
               RegGroup = cod + @"\d{2}000000000";
               break;
            }
            case 5:
            {
               RegGroup = cod + @"\d00000000" + @"|" +
                          cod + @"0\d{2}000000" + @"|" +
                          cod + @"000\d00000" + @"|" +
                          cod + @"0000\d{2}000" + @"|" +
                          cod + @"000000\d{2}0" + @"|" +
                          cod + @"000\d{3}000" + @"|" +
                          cod + @"000000\d{3}";

               break;
            }
            case 6:
            {
               RegGroup = cod + @"\d{2}000000" + @"|" +
                          cod + @"00\d00000" + @"|" +
                          cod + @"000\d{2}000" + @"|" +
                          cod + @"00000\d{2}0" + @"|" +
                          cod + @"00\d{3}000" + @"|" +
                          cod + @"00000\d{3}";
               break;
            }
            case 7:
            {
               RegGroup = cod + @"\d000000" + @"|" +
                          cod + @"0\d00000" + @"|" +
                          cod + @"00\d{2}000" + @"|" +
                          cod + @"0000\d{2}0" + @"|" +
                          cod + @"0\d{3}000" + @"|" +
                          cod + @"0000\d{3}";
               break;
            }
            case 8:
            {
               RegGroup = cod + @"\d00000" + @"|" +
                          cod + @"0\d{2}000" + @"|" +
                          cod + @"000\d{2}0" + @"|" +
                          cod + @"\d{3}000" + @"|" +
                          cod + @"000\d{3}";
               break;
            }
            case 9:
            {
               RegGroup = cod + @"\d{2}000" + @"|" +
                          cod + @"00\d{2}0" + @"|" +
                          cod + @"000\d{3}";
               break;
            }
            case 11:
            {
               RegGroup = cod + @"\d{2}0";
               break;
            }
            default:
            {
               RegGroup = cod + @"\d{" + (14 - cod.Count()) + "}";
               break;
            }
         }

         return RegGroup;
      }
   }

   class RH
   {
      public string name { get; set; }
      public string code { get; set; }
      public bool hasChildren { get; set; }
   }
}