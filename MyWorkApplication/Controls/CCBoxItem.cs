namespace MyWorkApplication.Classes
{
    public class CCBoxItem
    {
        public CCBoxItem()
        {
        }

        public CCBoxItem(string name, int val)
        {
            this.Name = name;
            this.Value = val;
        }

        public int Value { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("name: '{0}', value: {1}", Name, Value);
        }
    }
}