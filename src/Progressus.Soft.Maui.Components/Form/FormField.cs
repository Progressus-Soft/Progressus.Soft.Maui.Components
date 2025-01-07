using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressus.Soft.Maui.Components;

public class FormField: View
{
    public string Name { get; set; }
    public Type Type { get; set; }
    public Label Label { get; set; }
    public ErrorLabel ErrorLabel { get; set; }
    public ErrorImage ErrorImage { get; set; }
    public View Input { get; set; }
    public FieldStatus FieldStatus { get; set; }
    public bool IsCustomField { get; set; } = false;
}

public enum FieldStatus
{
    Default,
    Ignored,
    IgnoreRender,
    IgnoreValidation
}
