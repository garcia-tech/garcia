using System.Text;

namespace Garcia.Application
{
    public class GarciaStringBuilder
    {
        protected StringBuilder Builder { get; set; }

        public GarciaStringBuilder(string value)
        {
            Builder = new StringBuilder(value);
        }

        public GarciaStringBuilder(params string[] value) : this('\0', value)
        {
        }

        public GarciaStringBuilder(char seperator, params string[] value)
        {
            if (value != null && value.Length != 0)
            {
                Builder = new StringBuilder(value[0]);

                if (value.Length > 1)
                {
                    for (int i = 1; i < value.Length; i++)
                    {
                        if (seperator != '\0' && i != value.Length - 1)
                        {
                            Builder.Append(seperator);
                        }

                        Builder.Append(value[i]);
                    }
                }
            }
            else
            {
                Builder = new StringBuilder();
            }
        }

        public GarciaStringBuilder()
            : this(string.Empty)
        {
        }

        public void Append(string value)
        {
            Builder.Append(value);
        }

        public void Append(char value)
        {
            Builder.Append(value);
        }

        public void Append(bool value)
        {
            Builder.Append(value);
        }

        public void Append(int value)
        {
            Builder.Append(value);
        }

        public void Append(double value)
        {
            Builder.Append(value);
        }

        public void Append(long value)
        {
            Builder.Append(value);
        }

        public void Append(short value)
        {
            Builder.Append(value);
        }

        public void Append(byte value)
        {
            Builder.Append(value);
        }

        public void Append(char[] value)
        {
            Builder.Append(value);
        }

        public void Append(decimal value)
        {
            Builder.Append(value);
        }

        public void Append(float value)
        {
            Builder.Append(value);
        }

        public void Append(object value)
        {
            Builder.Append(value);
        }

        public void Append(char[] value, int startIndex, int charCount)
        {
            Builder.Append(value, startIndex, charCount);
        }

        public void Append(string value, int startIndex, int count)
        {
            Builder.Append(value, startIndex, count);
        }

        public static GarciaStringBuilder operator +(GarciaStringBuilder builder, string text)
        {
            builder.Builder.Append(text);
            return builder;
        }

        public static GarciaStringBuilder operator +(GarciaStringBuilder builder, char text)
        {
            builder.Builder.Append(text);
            return builder;
        }

        public override string ToString()
        {
            return Builder.ToString();
        }
    }
}