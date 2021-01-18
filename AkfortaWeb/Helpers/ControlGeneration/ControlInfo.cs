using EECExchange.ControlGeneration.Controls;

namespace AkfortaWeb.Helpers.ControlGeneration
{
   public class ControlInfo
   {
      // Common
      public int OrderNum { get; set; }
      public string Caption { get; set; }
      public ControlType ControlType { get; set; }
      public ControlInfo[] Children { get; set; } //Not (SimpleControl And not Complex DataType)

      // SimpleControl
      public bool BreakLine { get; set; }
      public DataType DataType { get; set; }
      public int ColumnSpan { get; set; }
      public string ComplexTypeKeyProperty { get; set; }

      //SimpleControl and grid
      public string PropertyName { get; set; }

      //Grid
      public string TableItemCaption { get; set; }
   }

   public enum ControlType
   {
      SimpleControl,
      GroupBox,
      Grid
   }
}