using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressus.Soft.Maui.Components.Primitives;

public partial class TemplatedEntry : Entry, IContentView
{
    public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ContentView), null, propertyChanged: TemplateUtilities.OnContentChanged);

    public View Content
    {
        get { return (View)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    object IContentView.Content => Content;

    IView IContentView.PresentedContent => ((this as IControlTemplated).TemplateRoot as IView) ?? Content;

    public static readonly BindableProperty PaddingProperty = PaddingElement.PaddingProperty;
    public Thickness Padding
    {
        get => (Thickness)GetValue(PaddingElement.PaddingProperty);
        set => SetValue(PaddingElement.PaddingProperty, value);
    }

    public Size CrossPlatformArrange(Rect bounds)
    {
        throw new NotImplementedException();
    }

    public Size CrossPlatformMeasure(double widthConstraint, double heightConstraint)
    {
        throw new NotImplementedException();
    }
}
