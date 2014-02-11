using System.Windows;
using System.Windows.Data;

namespace Nexfi.Tracker.Common.ObjectModel.Entities.ValidationConstraints
{
    public class ExpressionEvaluator
    {
        private class ExpressionPathTarget : DependencyObject
        {
            public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Value", typeof(object), typeof(ExpressionEvaluator));
            public object Value
            {
                get { return this.GetValue(ResultProperty); }
                //set { this.SetValue(ResultProperty, value); }
            }
        }

        public object GetValue(object source, string propertyPath)
        {
            var target = new ExpressionPathTarget();
            BindingOperations.SetBinding(target, ExpressionPathTarget.ResultProperty, new Binding(propertyPath)
            {
                Source = source,
                Mode = BindingMode.OneTime
            });
            return target.Value;
        }
    }
}
