using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nexfi.Tracker.Common.ObjectModel.Entities.ValidationConstraints
{
    public class ExpressionEnumerator : IEnumerable<KeyValuePair<string, string>>
    {
        private Type _sourceType;

        public ExpressionEnumerator(Type sourceType)
        {
            _sourceType = sourceType;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            var properties = _sourceType.GetProperties().Where(p => p.SetMethod != null).ToList();
            if (!properties.Any())
            {
                yield return new KeyValuePair<string, string>(string.Empty, string.Empty);
            }
            else
            {
                foreach (var propertyInfo in properties)
                {
                    if ((propertyInfo.Name != "RequiresUpdate" && propertyInfo.Name != "RequiresDeletion"))
                        yield return new KeyValuePair<string, string>(propertyInfo.Name, propertyInfo.PropertyType.AssemblyQualifiedName);
                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(ObjectBase)) && propertyInfo.PropertyType != _sourceType) //Prevents infinite loops in the event of a circular reference
                    {
                        var innerExplorer = new ExpressionEnumerator(propertyInfo.PropertyType);
                        foreach (KeyValuePair<string, string> innerPath in innerExplorer)
                        {
                            if (!string.IsNullOrEmpty(innerPath.Key))
                            {
                                if (innerPath.Key != "RequiresUpdate" && innerPath.Key != "RequiresDeletion")
                                    yield return new KeyValuePair<string, string>(propertyInfo.Name + "." + innerPath.Key, innerPath.Value);
                            }
                        }
                    }
                }
            }
        }

        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
