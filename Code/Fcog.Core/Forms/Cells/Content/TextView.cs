using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fcog.Core.Forms.Cells.Content
{
    [DataContract]
    public class TextView:IEquatable<TextView>
    {
        public TextView(string text)
        {
            Text = text;
        }

        [DataMember]
        public string Text { get; set; }

        #region IEquatable

        public bool Equals(TextView other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || string.Equals(Text, other.Text);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((TextView) obj);
        }

        public override int GetHashCode()
        {
            return (Text != null ? Text.GetHashCode() : 0);
        }

        #endregion

        public override string ToString()
        {
            return Text;
        }

        public static bool? ToBool(TextView textView)
        {
            bool? result = false;

            if (textView.Equals(TextViews.Miss))
            {
                result = null;
            }

            if (textView.Equals(TextViews.Mark))
            {
                result = true;
            }
           
            return result;
        }

        public static TextView FromBool(bool? boolValue)
        {
            var result = TextViews.Empty;
            switch (boolValue)
            {
                case true:
                {
                   result=TextViews.Mark;
                    break;
                }
                  
                case null:
                {
                    result = TextViews.Miss;
                    break;
                }
              
            }

            return result;
        }

        public static implicit operator TextView(string value)
        {
            return new TextView(value);
        }
    }
}