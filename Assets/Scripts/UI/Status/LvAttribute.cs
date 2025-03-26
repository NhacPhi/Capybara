using System.Text;
using Tech.Pooling;

namespace UI
{
    public class LvAttribute : AttributeTextValue
    {
        public const string Level = "Lv ";
        
        protected override void SetTextValue()
        {
            var stringBuilder = GenericPool<StringBuilder>.Get().Clear();
            stringBuilder.Append(Level);
            stringBuilder.Append(this.LastValue);
            textValue.text = stringBuilder.ToString();
            GenericPool<StringBuilder>.Return(stringBuilder);
        }
    }
}