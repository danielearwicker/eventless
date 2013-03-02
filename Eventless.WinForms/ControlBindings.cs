using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Eventless.WinForms
{
    public static class ControlBindings
    {
        public static void BindChanged<T>(IReadable<T> readable, Action changed)
        {
            changed();
            readable.Changed += changed;
            Binding.Log.Notify(() => readable.Changed -= changed);
        }

        public static void BindEnabled(this Control control, IReadable<bool> to)
        {
            BindChanged(to, () => control.Enabled = to.Value);
        }

        public static void BindVisible(this Control control, IReadable<bool> to)
        {
            BindChanged(to, () => control.Visible = to.Value);
        }

        public static void BindText(this TextBox textBox, IReadable<string> to)
        {
            BindChanged(to, () => textBox.Text = to.Value);
            textBox.ReadOnly = true;
        }

        public static void BindText(this TextBox textBox, IWriteable<string> to)
        {
            BindChanged(to, () => textBox.Text = to.Value);

            EventHandler handler = (sender, args) => to.Value = textBox.Text;
            textBox.TextChanged += handler;
            Binding.Log.Notify(() => textBox.TextChanged -= handler);
        }

        public static void BindText(this ButtonBase textBox, IReadable<string> to)
        {
            BindChanged(to, () => textBox.Text = to.Value);
        }

        public static void BindChecked(this CheckBox checkBox, IWriteable<bool> to)
        {
            BindChanged(to, () => checkBox.Checked = to.Value);

            EventHandler handler = (sender, args) => to.Value = checkBox.Checked;
            checkBox.CheckedChanged += handler;
            Binding.Log.Notify(() => checkBox.CheckedChanged -= handler);
        }

        public static void BindCheckState(this CheckBox checkBox, IWriteable<CheckState> to)
        {
            BindChanged(to, () => checkBox.CheckState = to.Value);

            EventHandler handler = (sender, args) => to.Value = checkBox.CheckState;
            checkBox.CheckStateChanged += handler;
            Binding.Log.Notify(() => checkBox.CheckStateChanged -= handler);
        }

        public static void BindChecked<TValue>(this RadioButton radioButton, IWriteable<TValue> to, 
                                               TValue value, Func<TValue, TValue, bool> areEqual = null)
        {
            if (areEqual == null)
                areEqual = Writeable<TValue>.DefaultEqualityComparer;

            BindChanged(to, () => radioButton.Checked = areEqual(to.Value, value));

            EventHandler handler = (sender, args) =>
                {
                    if (radioButton.Checked)
                        to.Value = value;
                };
            radioButton.CheckedChanged += handler;
            Binding.Log.Notify(() => radioButton.CheckedChanged -= handler);
        }

        public static void BindSelectedIndex(this ListBox listBox, IWriteable<int> to)
        {
            BindChanged(to, () => listBox.SelectedIndex = to.Value);

            EventHandler handler = (sender, args) => to.Value = listBox.SelectedIndex;
            listBox.SelectedIndexChanged += handler;
            Binding.Log.Notify(() => listBox.SelectedIndexChanged -= handler);
        }

        public static void BindForEach<TItem>(this CheckedListBox checkedListBox, 
                                            IWriteableList<TItem> to,
                                            Action<TItem, CheckedListBoxItem> bindItem)
        {
            checkedListBox.Items.Clear();
            foreach (var data in to.Value)
            {
                var newItem = new CheckedListBoxItem(checkedListBox);
                checkedListBox.Items.Add(newItem);
                bindItem(data, newItem);
            }

            to.Added += index =>
                {
                    var newItem = new CheckedListBoxItem(checkedListBox);
                    checkedListBox.Items.Insert(index, newItem);
                    bindItem(to[index], newItem);
                };
            to.Updated += index =>
                {
                    var newItem = new CheckedListBoxItem(checkedListBox);
                    var oldItem = ((CheckedListBoxItem)checkedListBox.Items[index]);
                    checkedListBox.Items[index] = newItem;
                    bindItem(to[index], newItem);
                    oldItem.Detach();
                };
            to.Removed += index =>
                {
                    var oldItem = (CheckedListBoxItem)checkedListBox.Items[index];
                    checkedListBox.Items.RemoveAt(index);
                    oldItem.Detach();
                };
            to.Cleared += () =>
                {
                    var olds = checkedListBox.Items.OfType<CheckedListBoxItem>().ToList();
                    checkedListBox.Items.Clear();
                    foreach (var old in olds)
                        old.Detach();
                };

            checkedListBox.ItemCheck += (s, ev) => ((CheckedListBoxItem)checkedListBox.Items[ev.Index]).OnCheck(ev.NewValue);
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

        private static void RebindItems<TItem, TControl>(Control panel,
            IList<TItem> to, Action<TItem, TControl> bindItem, int start)
            where TControl : Control, new()
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
                ctrl.CaptureUnbind(() => bindItem(item, ctrl));
            }
        }

        public static void BindItems<TItem, TControl>(this Panel panel,
                IWriteableList<TItem> to, Action<TItem, TControl> bindItem)
            where TControl : Control, new()
        {
            panel.AutoScroll = true;
            panel.Controls.Clear();
            RebindItems(panel, to, bindItem, 0);

            to.Added += index => RebindItems(panel, to, bindItem, index);
            to.Updated += index => panel.Controls[index].CaptureUnbind(
                () => bindItem(to[index], (TControl)panel.Controls[index]));
            to.Removed += index => RebindItems(panel, to, bindItem, index);
            to.Cleared += () => RebindItems(panel, to, bindItem, 0);

            Binding.Log.Notify(() =>
                {
                    while (panel.Controls.Count != 0)
                    {
                        var last = panel.Controls.Count - 1;
                        panel.Controls[last].Unbind();
                        panel.Controls.RemoveAt(last);
                    }
                });
        }
    }
}
