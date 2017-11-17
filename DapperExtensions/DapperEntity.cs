using PropertyChanged;
using System.Collections.Generic;

namespace DapperExtensions
{
    [AddINotifyPropertyChangedInterfaceAttribute]
    public class DapperEntity
    {
        private Dictionary<string, int> _changedPropertyNames = new Dictionary<string, int>();
                                
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (!IsDapperSetter())
            {
                if (!_changedPropertyNames.ContainsKey(propertyName))
                    _changedPropertyNames.Add(propertyName, 0);
            }
        }

        internal void Updated()
        {
            _changedPropertyNames.Clear();
        }

        internal bool IsUpdated(string propertyName)
        {
            return _changedPropertyNames.ContainsKey(propertyName);
        }

        static bool IsDapperSetter()
        {
            //当前堆栈信息
#if COREFX
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(null, false);
#else
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
#endif
            System.Diagnostics.StackFrame[] sfs = st.GetFrames();

            for (int i = 2; i < sfs.Length; ++i)
            {
                var name = sfs[i].GetMethod().Module.Name;
                //Console.WriteLine("Name:" + name);
                if (name.Contains("Dapper"))
                    return true;
            }

            return false;
        }
    }
}
