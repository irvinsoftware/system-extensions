using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

namespace Irvin.Extensions.Windows.Forms
{
	public delegate TValue GetControlValue<in TControl, out TValue>(TControl control);
	public delegate TValue GetControlProperty<TControl, TValue>(TControl control, GetControlValue<TControl, TValue> getControlValueMethod);
	public delegate void VoidMethod<in T>(T input);
	public delegate void ControlAction<T>(T control, VoidMethod<T> action);

    public static class BackgroundWorkerFormExtensions
    {
        public static TValue GetControlProperty<TControl, TValue>(this TControl control, GetControlValue<TControl, TValue> getOperation)
            where TControl : Control
        {
            if (control.InvokeRequired)
            {
				return (TValue)control.Invoke(new GetControlProperty<TControl, TValue>(GetControlProperty), new object[] { control, getOperation });
            }
            else
            {
                return getOperation(control);
            }
        }

		public static void SetControlProperty<TControl>(this TControl control, VoidMethod<TControl> assignmentAction)
            where TControl : Control
        {
            if (control.InvokeRequired)
            {
				control.Invoke(new ControlAction<TControl>(SetControlProperty), new object[] { control, assignmentAction });
            }
            else
            {
                assignmentAction(control);
            }
        }

        public static void SetToolStripControlProperty<TControl, TValue>(this TControl control, Expression<Func<TControl, TValue>> targetExpression, TValue value)
            where TControl : ToolStripItem
        {
            Action<TControl, TValue> assignmentAction = (targetControl, actualValue) =>
            {
                Type controlType = targetControl.GetType();
                MemberExpression memberExpression = (MemberExpression)targetExpression.Body;
                PropertyInfo propertyInfo = controlType.GetProperty(memberExpression.Member.Name);
                propertyInfo.SetValue(targetControl, actualValue, null);
            };
            control.Owner.Parent.Invoke(new Action<TControl, TValue>(assignmentAction), control, value);
        }
    }
}