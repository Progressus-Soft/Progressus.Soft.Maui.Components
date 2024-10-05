using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressus.Soft.Maui.Components;

public class FormField
{
    public string Name { get; set; }
    public Type Type { get; set; }
    public Label Label { get; set; }
    public ErrorLabel ErrorLabel { get; set; }
    public View Input { get; set; }
    public FieldStatus FieldStatus { get; set; }
}

public enum FieldStatus
{
    Default,
    Ignored,
    IgnoreRender,
    IgnoreValidation
}
