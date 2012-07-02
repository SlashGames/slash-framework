namespace RainyGames.Collections.AttributeTables
{
    using System.Collections.Generic;

    public class AttributeTable
    {
        /// <summary>
        ///   Dictionary to store attributes.
        /// </summary>
        private readonly Dictionary<object, object> attributes = new Dictionary<object, object>();

        /// <summary>
        ///   Copy constructor.
        /// </summary>
        /// <param name="original">Attribute table to copy.</param>
        public AttributeTable(AttributeTable original)
        {
            this.CopyAttributes(original);
        }

        public AttributeTable()
        {
        }

        public void SetValue(object key, object value)
        {
            this.attributes[key] = value;
        }

        public void Remove(object key)
        {
            this.attributes.Remove(key);
        }

        public void Add(object key, object value)
        {
            this.attributes.Add(key, value);
        }

        private void CopyAttributes(AttributeTable original)
        {
            foreach (KeyValuePair<object, object> keyValuePair in original.attributes)
            {
                this.attributes.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        protected bool ContainsKey(object id)
        {
            return this.attributes.ContainsKey(id);
        }

        protected bool TryGetValue(object id, out object value)
        {
            return this.attributes.TryGetValue(id, out value);
        }
    }
}