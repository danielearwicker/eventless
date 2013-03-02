using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Eventless.WinForms
{
    public static class ControlBindings
    {
        public static Action Throttle(int milliseconds, Action action)
        {
            Timer timer = null;

            return () =>
            {
                if (timer != null)
                    return;

                timer = new Timer { Interval = milliseconds };
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                    action();
                };
                timer.Start();
            };
        }

        public static void BindChanged<T>(IGetable<T> getable, Action changed)
        {
            changed();
            getable.Changed += changed;
            Binding.Log.Notify(() => getable.Changed -= changed);
        }

        public static void BindEnabled(this Control control, IGetable<bool> to)
        {
            BindChanged(to, () => control.Enabled = to.Value);
        }

        public static void BindVisible(this Control control, IGetable<bool> to)
        {
            BindChanged(to, () => control.Visible = to.Value);
        }

        public static void BindText(this TextBox textBox, IGetable<string> to)
        {
            BindChanged(to, () => textBox.Text = to.Value);
            textBox.ReadOnly = true;
        }

        public static void BindText(this TextBox textBox, ISetable<string> to)
        {
            BindChanged(to, () => textBox.Text = to.Value);

            EventHandler handler = (sender, args) => to.Value = textBox.Text;
            textBox.TextChanged += handler;
            Binding.Log.Notify(() => textBox.TextChanged -= handler);
        }

        public static void BindText(this ButtonBase textBox, IGetable<string> to)
        {
            BindChanged(to, () => textBox.Text = to.Value);
        }

        public static void BindChecked(this CheckBox checkBox, ISetable<bool> to)
        {
            BindChanged(to, () => checkBox.Checked = to.Value);

            EventHandler handler = (sender, args) => to.Value = checkBox.Checked;
            checkBox.CheckedChanged += handler;
            Binding.Log.Notify(() => checkBox.CheckedChanged -= handler);
        }

        public static void BindCheckState(this CheckBox checkBox, ISetable<CheckState> to)
        {
            BindChanged(to, () => checkBox.CheckState = to.Value);

            EventHandler handler = (sender, args) => to.Value = checkBox.CheckState;
            checkBox.CheckStateChanged += handler;
            Binding.Log.Notify(() => checkBox.CheckStateChanged -= handler);
        }

        public static void BindChecked<TValue>(this RadioButton radioButton, ISetable<TValue> to, 
                                               TValue value, Func<TValue, TValue, bool> areEqual = null)
        {
            if (areEqual == null)
                areEqual = Setable<TValue>.DefaultEqualityComparer;

            BindChanged(to, () => radioButton.Checked = areEqual(to.Value, value));

            EventHandler handler = (sender, args) =>
                {
                    if (radioButton.Checked)
                        to.Value = value;
                };
            radioButton.CheckedChanged += handler;
            Binding.Log.Notify(() => radioButton.CheckedChanged -= handler);
        }

        public static void BindSelectedIndex(this ListBox listBox, ISetable<int> to)
        {
            BindChanged(to, () => listBox.SelectedIndex = to.Value);

            EventHandler handler = (sender, args) => to.Value = listBox.SelectedIndex;
            listBox.SelectedIndexChanged += handler;
            Binding.Log.Notify(() => listBox.SelectedIndexChanged -= handler);
        }

        // Just used as a wrapper type to police use of Tag property
        private class UnbindAction
        {
            internal Action Action;
        }

        public static void CaptureUnbind(this Control control, Action bindItem)
        {
            Unbind(control);
            control.Tag = new UnbindAction
                {
                    Action = Binding.CaptureUnbind(bindItem)
                };
        }

        public static void Unbind(this Control control)
        {
            var unbindAction = (UnbindAction)control.Tag; // Give up if Tag already being used for something else
            control.Tag = null;
            if (unbindAction != null)
                unbindAction.Action();
        }

        private static void RebindItems<TItem, TControl>(Control panel, IList<TItem> to, int start)
            where TControl : Control, IBindsTo<TItem>, new()
        {
            // If we have too many controls, remove from the end
            while (panel.Controls.Count > to.Count)
            {
                var last = panel.Controls.Count - 1;
                var ctrl = panel.Controls[last];
                ctrl.Unbind();
                panel.Controls.RemoveAt(last);
            }

            // If too few, add to the end
            while (panel.Controls.Count < to.Count)
            {
                var ctrl = new TControl();
                var last = panel.Controls.Count - 1;
                if (last >= 0)
                    ctrl.Top = panel.Controls[last].Bottom;
                panel.Controls.Add(ctrl);
            }

            // Rebind all controls starting from 'start' position
            for (var i = start; i < to.Count; i++)
            {
                var item = to[i];
                var ctrl = (TControl) panel.Controls[i];
                ctrl.CaptureUnbind(() => ctrl.Bind(item));
            }
        }

        public class ForEachBinder<TItem>
        {
            private readonly Panel _panel;
            private readonly ISetableList<TItem> _to;

            internal ForEachBinder(ISetableList<TItem> to, Panel panel)
            {
                _to = to;
                _panel = panel;
            }

            public void As<TControl>()
                where TControl : Control, IBindsTo<TItem>, new()
            {
                _panel.AutoScroll = true;
                _panel.Controls.Clear();
                RebindItems<TItem, TControl>(_panel, _to, 0);

                var throttledResize = Throttle(100, () =>
                {
                    foreach (var control in _panel.Controls.OfType<Control>())
                        control.Width = _panel.ClientSize.Width - 1;
                });

                EventHandler onResize = (s, ev) => throttledResize();

                _to.Added += index =>
                {
                    RebindItems<TItem, TControl>(_panel, _to, index);
                    onResize(null, null);
                };
                _to.Updated += index => _panel.Controls[index].CaptureUnbind(
                    () => ((TControl)_panel.Controls[index]).Bind(_to[index]));
                _to.Removed += index => RebindItems<TItem, TControl>(_panel, _to, index);
                _to.Cleared += () => RebindItems<TItem, TControl>(_panel, _to, 0);

                onResize(null, null);
                _panel.Resize += onResize;

                Binding.Log.Notify(() =>
                {
                    _panel.Resize -= onResize;

                    while (_panel.Controls.Count != 0)
                    {
                        var last = _panel.Controls.Count - 1;
                        _panel.Controls[last].Unbind();
                        _panel.Controls.RemoveAt(last);
                    }
                });
            }   
        }

        public static ForEachBinder<TItem> BindForEach<TItem>(this Panel panel, ISetableList<TItem> to)
        {
            return new ForEachBinder<TItem>(to, panel);
        }
    }
}
