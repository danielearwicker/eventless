using System;
using System.Windows.Forms;

namespace Eventless.WinForms
{
    public class CheckedListBoxItem
    {
        private string _text;
        private CheckedListBox _owner;
        private Action<CheckState> _checked;

        internal CheckedListBoxItem(CheckedListBox owner)
        {
            _owner = owner;
        }

        internal void Detach()
        {
            _owner = null;
        }

        public void BindText(IReadable<string> to)
        {
            _text = to.Value;
            to.Changed += () =>
                {
                    if (_owner != null)
                    {
                        _text = to.Value;
                        _owner.Invalidate();
                    }
                };
        }

        internal void OnCheck(CheckState check)
        {
            if (_owner != null)
            {
                var checkEvt = _checked;
                if (checkEvt != null)
                    checkEvt(check);
            }
        }

        public void BindChecked(IWriteable<bool> to)
        {
            _owner.SetItemChecked(_owner.Items.IndexOf(this), to.Value);
            to.Changed += () =>
                {
                    if (_owner != null)
                        _owner.SetItemChecked(_owner.Items.IndexOf(this), to.Value);
                };
            _checked += (newState) => to.Value = newState == CheckState.Checked;
        }

        public void BindCheckState(IWriteable<CheckState> to)
        {
            _owner.SetItemCheckState(_owner.Items.IndexOf(this), to.Value);
            to.Changed += () =>
                {
                    if (_owner != null)
                        _owner.SetItemCheckState(_owner.Items.IndexOf(this), to.Value);
                };
            _checked += (newState) => to.Value = newState;
        }

        public override string ToString()
        {
            return _text ?? string.Empty;
        }
    }
}