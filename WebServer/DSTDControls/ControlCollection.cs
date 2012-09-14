using System.Collections;
using System.Collections.Generic;

namespace DSTDControls
{
    public class ControlCollection:IList<Control>
    {
        private List<Control> Controls = new List<Control>();

        public IEnumerator<Control> GetEnumerator()
        {
            return Controls.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Control item)
        {
            item.Parent = me;
            Controls.Add(item);
        }

        public void Clear()
        {
            Controls.Clear();
        }

        public bool Contains(Control item) {
            return Controls.Contains(item);
        }


        public void CopyTo(Control[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(Control item)
        {
            return            Controls.Remove(item);
        }

        public int Count
        {
            get { return Controls.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(Control item)
        {
            return            Controls.IndexOf(item);
        }

        public void Insert(int index, Control item)
        {
            Controls.Insert(index,item);
        }

        public void RemoveAt(int index)
        {
            Controls.RemoveAt(index);
        }

        public Control this[int index] {
            get {
                return Controls[index];
            }
            set { Controls[index] = value; }

        }

        private Control me;
        public ControlCollection(Control c)
        {
            me = c;
        }
    }
}