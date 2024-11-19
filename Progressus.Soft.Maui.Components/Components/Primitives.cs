using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressus.Soft.Maui.Components.Primitives;
internal static class TemplateUtilities
{
    public static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        IControlTemplated controlTemplated = (IControlTemplated)bindable;
        Element element = (Element)newValue;
        if (controlTemplated.ControlTemplate == null)
        {
            while (controlTemplated.InternalChildren.Count > 0)
            {
                controlTemplated.InternalChildren.RemoveAt(controlTemplated.InternalChildren.Count - 1);
            }

            if (newValue != null)
            {
                controlTemplated.InternalChildren.Add(element);
            }
        }
        else if (element != null)
        {
            BindableObject.SetInheritedBindingContext(element, bindable.BindingContext);
        }
    }
    
    public static object GetTemplateChild(this IControlTemplated controlTemplated, string name)
    {
        return controlTemplated.TemplateRoot?.FindByName(name);
    }

    internal static void OnChildRemoved(IControlTemplated controlTemplated, Element removedChild)
    {
        if (removedChild == controlTemplated.TemplateRoot)
        {
            controlTemplated.TemplateRoot = null;
        }
    }
}
internal interface IControlTemplated
{
    ControlTemplate ControlTemplate { get; set; }

    IList<Element> InternalChildren { get; }

    Element TemplateRoot { get; set; }

    void OnControlTemplateChanged(ControlTemplate oldValue, ControlTemplate newValue);

    void OnApplyTemplate();
}
internal static class PaddingElement
{
    //
    // Resumen:
    //     Bindable property for Microsoft.Maui.Controls.IPaddingElement.Padding.
    public static readonly BindableProperty PaddingProperty = BindableProperty.Create("Padding", typeof(Thickness), typeof(IPaddingElement), default(Thickness), BindingMode.OneWay, null, OnPaddingPropertyChanged, null, null, PaddingDefaultValueCreator);

    //
    // Resumen:
    //     Bindable property for attached property PaddingLeft.
    public static readonly BindableProperty PaddingLeftProperty = BindableProperty.Create("PaddingLeft", typeof(double), typeof(IPaddingElement), 0.0, BindingMode.OneWay, null, OnPaddingLeftChanged);

    //
    // Resumen:
    //     Bindable property for attached property PaddingTop.
    public static readonly BindableProperty PaddingTopProperty = BindableProperty.Create("PaddingTop", typeof(double), typeof(IPaddingElement), 0.0, BindingMode.OneWay, null, OnPaddingTopChanged);

    //
    // Resumen:
    //     Bindable property for attached property PaddingRight.
    public static readonly BindableProperty PaddingRightProperty = BindableProperty.Create("PaddingRight", typeof(double), typeof(IPaddingElement), 0.0, BindingMode.OneWay, null, OnPaddingRightChanged);

    //
    // Resumen:
    //     Bindable property for attached property PaddingBottom.
    public static readonly BindableProperty PaddingBottomProperty = BindableProperty.Create("PaddingBottom", typeof(double), typeof(IPaddingElement), 0.0, BindingMode.OneWay, null, OnPaddingBottomChanged);

    private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((IPaddingElement)bindable).OnPaddingPropertyChanged((Thickness)oldValue, (Thickness)newValue);
    }

    private static object PaddingDefaultValueCreator(BindableObject bindable)
    {
        return ((IPaddingElement)bindable).PaddingDefaultValueCreator();
    }

    private static void OnPaddingLeftChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Thickness thickness = (Thickness)bindable.GetValue(PaddingProperty);
        thickness.Left = (double)newValue;
        bindable.SetValue(PaddingProperty, thickness);
    }

    private static void OnPaddingTopChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Thickness thickness = (Thickness)bindable.GetValue(PaddingProperty);
        thickness.Top = (double)newValue;
        bindable.SetValue(PaddingProperty, thickness);
    }

    private static void OnPaddingRightChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Thickness thickness = (Thickness)bindable.GetValue(PaddingProperty);
        thickness.Right = (double)newValue;
        bindable.SetValue(PaddingProperty, thickness);
    }

    private static void OnPaddingBottomChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Thickness thickness = (Thickness)bindable.GetValue(PaddingProperty);
        thickness.Bottom = (double)newValue;
        bindable.SetValue(PaddingProperty, thickness);
    }
}
