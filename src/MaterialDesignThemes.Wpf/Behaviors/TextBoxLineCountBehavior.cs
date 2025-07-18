﻿using System.Windows.Threading;
using Microsoft.Xaml.Behaviors;

namespace MaterialDesignThemes.Wpf.Behaviors;

/// <summary>
/// Behavior exposing the <see cref="TextBox.LineCount"/> (non-DP) as attached properties which are bindable from XAML.
/// </summary>
public class TextBoxLineCountBehavior : Behavior<TextBox>
{
    private void AssociatedObjectOnTextChanged(object sender, TextChangedEventArgs e) => UpdateAttachedProperties();
    private void AssociatedObjectOnLayoutUpdated(object? sender, EventArgs e) => UpdateAttachedProperties();

    private void UpdateAttachedProperties()
    {
        if (AssociatedObject is { } associatedObject)
        {
            associatedObject.Dispatcher
                .BeginInvoke(() =>
                {
                    int lineCount = associatedObject.LineCount;
                    associatedObject.SetCurrentValue(TextFieldAssist.TextBoxLineCountProperty, lineCount);
                    associatedObject.SetCurrentValue(TextFieldAssist.TextBoxIsMultiLineProperty, lineCount > 1);
                },
                DispatcherPriority.Background);
        }
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        AssociatedObject.LayoutUpdated += AssociatedObjectOnLayoutUpdated;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
            AssociatedObject.LayoutUpdated -= AssociatedObjectOnLayoutUpdated;
        }
        base.OnDetaching();
    }
}
